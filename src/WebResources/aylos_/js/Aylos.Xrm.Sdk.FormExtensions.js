if (typeof Aylos === typeof undefined) var Aylos = {}
if (typeof Aylos.Xrm === typeof undefined) Aylos.Xrm = {}
if (typeof Aylos.Xrm.Sdk === typeof undefined) Aylos.Xrm.Sdk = {}
if (typeof Aylos.Xrm.Sdk.FormExtensions === typeof undefined) {
    Aylos.Xrm.Sdk.FormExtensions = (function () {
        /**
         * Form extensions library to be used across all modules. Please remember to
         * minify/compress the library when releasing in CRM for improved performance.
         * JavaScript compression removes all comments, debugger commands and replaces
         * the variable names with shorter names.
         */
        "use strict";

        const _constants = {
            errorMessage: "An error occured during execution press CTRL-C to copy the error "
                + "to the clipboard, then create a new e-mail message and press CTRL-V to paste "
                + "the error message into the e-mail. Then submit it to the CRM support team.\n",
        }

        const _formType = {
            CREATE: 1,
            UPDATE: 2,
            READONLY: 3,
            DISABLED: 4,
            QUICKCREATE: 5,
            BULKEDIT: 6,
        }

        const _notificationLevel = {
            ERROR: "ERROR",
            WARNING: "WARNING",
            INFO: "INFO",
        }

        const _saveEventMode = {
            SAVE: 1,
            SAVEANDCLOSE: 2,
            SAVEANDNEW: 59,
            AUTOSAVE: 70,
            SAVEASCOMPLETED: 58, // Activities only
            DEACTIVATE: 5,
            REACTIVATE: 6,
            ASSIGN: 47, // User or Team owned entities
            SEND: 7, // Email (E-mail)
            QUALIFY: 16, // Lead
            DISQUALIFY: 15, // Lead
        }

        /* Variables */
        var _properties = {
            asyncCallsInProgress: 0,
            formType: null,
            formLabel: null,
            entityId: null,
            userId: null,
        }

        /* It must be initialised by calling the initialisation method. */
        var _executionContext = typeof Xrm.Page === typeof undefined ? null : Xrm.Page; 

        /* It must be initialised by calling the initialisation method. */
        var _formContext = _executionContext === null ? null : _executionContext.context;

        /**
         * Displays a confirmation dialog containing a message and buttons.
         * 
         * @param {!string} text The text to render.
         * @param {?string} [title] The title of the window dialog. 
         * @param {?string} [subtitle] The sub title of the window dialog.
         * @param {?string} [confirmButtonLabel] The confirm button label.
         * @param {?string} [cancelButtonLabel] The cancel button label.
         * @param {?Function} [successCallback] A callback reference to be called on save success.
         * @param {?Function} [errorCallback] A callback reference to be called on save error.
         *
         */
        const _confirmDialog = function (text, title, subtitle, confirmButtonLabel, cancelButtonLabel, successCallback, errorCallback) {

            if (text == null) throw new Error("The text argument is undefined.");
            if (typeof text !== "string") throw new Error("The text argument type must be a string.");

            let confirmStrings = {
                cancelButtonLabel: cancelButtonLabel,
                confirmButtonLabel: confirmButtonLabel,
                subtitle: subtitle,
                text: text,
                title: title
            }

            let confirmOptions = {
                height: 300,
                width: 500
            };

            Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
                function (confirm) {
                    if (typeof successCallback === "function") successCallback(confirm);
                },
                function (error) {
                    if (typeof errorCallback === "function") errorCallback(error);
                }
            );
        }

        /**
         * Private function to convert matching string values to Date objects.
         * Try using ISO formatted dates (yyyy-MM-ddTHH:mm:ssZ).
         * 
         * @param {?string} value The string value representing a date.
         * @returns {Date} 
         * // or:
         * @returns {string} The specified input value.
         * 
         */
        const _dateReviver = function (value) {

            if (this.IsNullOrWhiteSpace(value) || typeof value !== "string") return value;

            let date = Date.parse(value);
            if (!isNaN(date)) return new Date(date);
        }

        /**
         * Enables or disables all related controls for a given attribute.
         * 
         * @param {!string} controlName The attribute logical name.
         * @param {!boolean} disabled The attribute disabled or enabled value.
         * @param {?string} [displayName] The attribute display name.
         */
        const _disableControl = function (controlName, disabled, displayName) {

            if (this.IsNullOrWhiteSpace(controlName)) throw new Error("The control name argument is undefined.");
            if (typeof controlName !== "string") throw new Error("The control name argument type must be a string.");

            if (this.IsNull(disabled)) throw new Error("The disabled argument is undefined.");
            if (typeof disabled !== "boolean") throw new Error("The disabled argument type must be a boolean.");

            let attribute = this.GetAttribute(controlName, displayName);

            attribute.controls.forEach(function (control, index) {
                control.setDisabled(disabled);
            });
        }

        /**
         * Fix the process and stage when those are not the same with the active ones.
         */
        const _fixProcessAndStage = function () {

            let processIdAttr = this.GetAttribute("processid");
            if (this.IsNullOrWhiteSpace(processIdAttr)) return;

            let stageIdAttr = this.GetAttribute("stageid");
            if (this.IsNullOrWhiteSpace(stageIdAttr)) return;

            let activeProcess = this.FormContext.data.process.getActiveProcess();
            if (this.IsNull(activeProcess)) throw new Error("Active process is undefined.");

            let processId = processIdAttr.getValue();
            let stageId = stageIdAttr.getValue();
            let activeProcessId = activeProcess.getId();

            if (this.GuidsAreEqual(processId, activeProcessId)) return;

            this.FormContext.data.process.setActiveProcess(processId,
                function (result) {
                    switch (result) {
                        case "success":
                            this.FormContext.data.process.setActiveStage(stageId);
                            break;
                    }
                });
        }

        /**
         * Returns an attribute instance for the given attribute name. Throws an error 
         * if the attribute does not exist on the form.
         * 
         * @param {!string} attributeName The attribute logical name.
         * @param {?string} [displayName] The attribute display name.
         * 
         */
        const _getAttribute = function (attributeName, displayName) {

            if (this.IsNullOrWhiteSpace(attributeName)) throw new Error("The attribute name argument is undefined.");
            if (typeof attributeName !== "string") throw new Error("The attribute name argument type must be a string.");

            let label = this.IsNullOrWhiteSpace(displayName) ? attributeName : displayName;

            let attribute = this.FormContext.getAttribute(attributeName);
            if (attribute == null) throw new Error("The attribute '" + label + "' does not exist on the form.");

            return attribute;
        }

        /**
         * Returns a XRM attribute value for the given attribute name. Throws an error 
         * if the attribute does not exist on the form.
         * 
         * @param {!string} attributeName The attribute logical name.
         * @param {?string} [displayName] The attribute display name.
         *
         */
        const _getAttributeValue = function (attributeName, displayName) {

            let attribute = this.GetAttribute(attributeName, displayName);

            return attribute.getValue();
        }

        /**
         * Returns a XRM form control. Throws an error
         * if the control does not exist on the form.
         * 
         * @param {!string} controlName The control name.
         * @param {?string} [displayName] The control display name.
         *
         */
        const _getControl = function (controlName, displayName) {

            if (this.IsNullOrWhiteSpace(controlName)) throw new Error("The control name argument is undefined.");
            if (typeof controlName !== "string") throw new Error("The control name argument type must be a string.");

            let label = this.IsNullOrWhiteSpace(displayName) ? controlName : displayName;

            let control = this.FormContext.getControl(controlName);
            if (this.IsNull(control)) throw new Error("The control '" + label + "' does not exist on the form.");

            return control;
        }

        /**
         *  Returns the label of the current form.
         */
        const _getFormLabel = function () {

            let currentForm = this.FormContext.ui.formSelector.getCurrentItem();
            if (this.IsNull(currentForm)) throw new Error("Current form is undefined.");

            return currentForm.getLabel();
        }

        /** 
         *  Returns the type of the form.
         */
        const _getFormType = function () {

            return this.FormContext.ui.getFormType();
        }

        /**
         * Returns a section instance for the given tab and section name. Throws an error 
         * if the tab or the section does not exist on the form.
         * 
         * @param {!string} tabName The tab logical name.
         * @param {!string} sectionName The tab logical name.
         * @param {?string} [displayName] The tab display name.
         * @returns {Object} The section object.
         * 
         */
        const _getSection = function (tabName, sectionName, displayName) {

            if (this.IsNullOrWhiteSpace(tabName)) throw new Error("The tab name argument is undefined.");
            if (typeof tabName !== "string") throw new Error("The tab name argument type must be a string.");

            if (this.IsNullOrWhiteSpace(sectionName)) throw new Error("The section name argument is undefined.");
            if (typeof sectionName !== "string") throw new Error("The section name argument type must be a string.");

            let tab = this.GetTab(tabName);
            if (tab == null) throw new Error("The tab '" + label + "' does not exist on the form.");

            let label = this.IsNullOrWhiteSpace(displayName) ? sectionName : displayName;

            let section = tab.sections.get(sectionName);
            if (section == null) throw new Error("The section '" + label + "' does not exist on the form.");

            return section;
        }

        /**
         * Cleans up a Guid from spaces, curly brackets and returns a lowercase 
         * version of it useful for comparisons.
         * 
         * @param {any} guid
         */
        const _getStrippedGuid = function (guid) {

            if (this.IsNullOrWhiteSpace(guid)) throw new Error("The guid argument is undefined.");
            if (typeof guid !== "string") throw new Error("The guid argument type must be a string.");

            return guid
                .replace(" ", "")
                .replace("{", "")
                .replace("}", "")
                .replace("[", "")
                .replace("]", "")
                .toLowerCase();
        }

        /**
         * Returns a tab instance for the given tab name. Throws an error 
         * if the tab does not exist on the form.
         * 
         * @param {!string} tabName The tab logical name.
         * @param {?string} [displayName] The tab display name.
         * @returns {Object} The tab object.
         * 
         */
        const _getTab = function (tabName, displayName) {

            if (this.IsNullOrWhiteSpace(tabName)) throw new Error("The tab name argument is undefined.");
            if (typeof tabName !== "string") throw new Error("The tab name argument type must be a string.");

            let label = this.IsNullOrWhiteSpace(displayName) ? tabName : displayName;

            let tab = this.FormContext.ui.tabs.get(tabName);
            if (tab == null) throw new Error("The tab '" + label + "' does not exist on the form.");

            return tab;
        }

        /**
         * Compares two Guids and returns true if they are equal.
         * 
         * @param {!string} guid1 The first GUID to compare.
         * @param {!string} guid2 The second GUID to compare.
         * @returns {boolean}
         * 
         */
        const _guidsAreEqual = function (guid1, guid2) {

            if (typeof guid1 === typeof undefined) throw new Error("The guid1 argument is undefined.");
            if (typeof guid1 !== "string") throw new Error("The guid1 argument type must be a string.");

            if (typeof guid2 === typeof undefined) throw new Error("The guid2 argument is undefined.");
            if (typeof guid2 !== "string") throw new Error("The guid2 argument type must be a string.");

            return this.GetStrippedGuid(guid1) === this.GetStrippedGuid(guid2);
        }

        /**
         * Handles an error by rendering the information on the user's interface.
         * 
         * @param {!Error} error The actual error to process. 
         * 
         */
        const _handleException = function (error) {

            if (typeof error === typeof undefined) throw new Error("The error argument is undefined.");
            if (typeof error !== "object") throw new Error("The error argument type must be an object.");

            let message = this.Constants.errorMessage; if (!this.IsNullOrWhiteSpace(error.message)) message = error.message;

            let details = this.StrignifyException(error);

            this.ShowError(message, details);
        }

        /**
         * Initialises the JavaScript library by specifying the execution context.
         * 
         * @param {!Object} executionContext The execution context defines the event context in which your code executes.
         * @param {?Object} [xrm] Returns itself updated with the ExecutionContext and FormContext.
         * 
         */
        const _initialize = function (executionContext, xrm) {

            if (this.IsNull(executionContext)) throw new Error("The execution context argument is undefined.");
            if (typeof executionContext !== "object") throw new Error("The execution context argument type must be an object.");

            this.ExecutionContext = executionContext;
            this.FormContext = executionContext.getFormContext();

            xrm = this;
        }

        /**
          * Checks if an argument is undefined or NULL.
          * 
          * @param  {any} obj the input to check.
          * @returns {boolean} true if satisfies the criteria.
          * 
          */
        const _isNull = function (obj) {

            return typeof obj === typeof undefined || obj === null;
        }

        /**
          * Checks if a string is undefined or NULL or empty or whitespace.
          * 
          * @param  {string} val The string to check.
          * @returns {boolean} true if satisfies the criteria.
          */
        const _isNullOrWhiteSpace = function (val) {

            if (typeof val !== "undefined" && typeof val !== "string") return this.IsNull(val);
            return typeof val === typeof undefined || val === null || val.match(/^ *$/) !== null;
        }

        /**
         * Asynchronously refreshes and optionally saves all the data of the form without reloading the page.
         * 
         * @param {!boolean} save A boolean value to indicate if data should be saved after it is refreshed.
         * @param {?Function} [successCallback] A callback reference to be called on save success.
         * @param {?Function} [errorCallback] A callback reference to be called on save error.
         * 
         */
        const _refreshForm = function (save, successCallback, errorCallback) {

            if (this.IsNull(save)) throw new Error("The save argument is undefined.");
            if (typeof save !== "boolean") throw new Error("The save argument type must be a boolean.");

            this.FormContext.data.refresh(save).then(
                function (result) {
                    if (typeof successCallback === "function") successCallback(result);
                },
                function (error) {
                    if (typeof errorCallback === "function") errorCallback(error);
                }
            );
        }

        /**
         * Causes the ribbon to re-evaluate data that controls what is displayed in it. Triggers a refresh 
         * ribbon action when data that the ribbon depends get changed. Call the function in the code or 
         * as part of the on-change event of a particular field.
         * 
         * @param {?boolean} [refreshAll] Indicates whether all the ribbon command bars on the current page are refreshed.
         * If you specify false, only the page-level ribbon command bar is refreshed. If you do not specify this 
         * parameter, by default false is passed.
         * 
         */
        const _refreshRibbon = function (refreshAll) {

            if (!this.IsNull(refreshAll) && typeof refreshAll !== "boolean") throw new Error("The refreshAll argument type must be a boolean.");

            this.FormContext.ui.refreshRibbon(!this.IsNull(refreshAll) || refreshAll);
        }

        /**
         * Replaces all instances of the search text in the specified string
         * with the replace text. 
         * 
         * @param {any} str The specified string to modify.
         * @param {any} search The text to match and replace in the given string.
         * @param {any} replace The text to use as a replacement in the given string.
         * 
         * https://regex101.com/codegen?language=javascript
         * 
         */
        const _replaceText = function (str, search, replace) {

            if (this.IsNullOrWhiteSpace(str)) throw new Error("The str argument is undefined.");
            if (typeof str !== "string") throw new Error("The str argument type must be a boolean.");

            if (this.IsNullOrWhiteSpace(search)) throw new Error("The search argument is undefined.");
            if (typeof search !== "string") throw new Error("The search argument type must be a boolean.");

            if (this.IsNull(replace)) throw new Error("The replace argument is undefined.");
            if (typeof replace !== "string") throw new Error("The replace argument type must be a boolean.");

            var regex = new RegExp(search, "g");
            return str.replace(regex, replace);
        }

        /**
         * Saves the record asynchronously with the option to set callback functions to be executed after the save operation is completed. 
         * You can also set an object to control how appointment, recurring appointment, or service activity records are processed.
         * 
         * @param {?Object} [saveOptions] An object for specifying options for saving the record. The object has following attributes: 
         * - saveMode: (Optional) Number. Specify a value indicating how the save event was initiated. For a list of supported values, 
         * see the return value of the getSaveMode method. Note that setting the saveMode does not actually take the corresponding action; 
         * it is just to provide information to the OnSave event handlers about the reason for the save operation. 
         * - useSchedulingEngine: (Optional) boolean. Indicate whether to use the Book or Reschedule messages rather than the Create or 
         * Update messages. This option is only applicable when used with appointment, recurring appointment, or service activity records.
         * @param {?Function} [successCallback] A callback reference to be called on save success.
         * @param {?Function} [errorCallback] A callback reference to be called on save error.
         *
         */
        const _saveForm = function (saveOptions, successCallback, errorCallback) {

            this.FormContext.data.refresh(saveOptions).then(
                function (result) {
                    if (typeof successCallback === "function") successCallback(result);
                },
                function (error) {
                    if (typeof errorCallback === "function") errorCallback(error);
                }
            );
        }

        /**
         * Shows or hides all children controls for a given attribute.
         * @param {!string} attributeName The attribute logical name.
         * @param {!boolean} visible The visibility state.
         * @param {?string} [displayName] The attribute display name.
         * 
         */
        const _setAttributeControlsVisibility = function (attributeName, visible, displayName) {

            if (this.IsNullOrWhiteSpace(attributeName)) throw new Error("The attribute name argument is undefined.");
            if (typeof attributeName !== "string") throw new Error("The attribute name argument type must be a string.");

            if (this.IsNull(disabled)) throw new Error("The disabled argument is undefined.");
            if (typeof disabled !== "boolean") throw new Error("The disabled argument type must be a boolean.");

            let attribute = this.GetAttribute(attributeName, displayName);

            attribute.controls.forEach(function (control, index) {
                control.setVisible(visible);
            });
        }

        /**
         * Sets whether data is required, optional or recommended for the given attribute.
         * 
         * @param {!string} attributeName The attribute logical name.
         * @param {!string} requiredLevel The required level to be set to one of the following values: none, required, recommended.
         * @param {?string} [displayName] The attribute display name.
         * 
         */
        const _setAttributeRequired = function (attributeName, requiredLevel, displayName) {

            if (this.IsNullOrWhiteSpace(attributeName)) throw new Error("The attribute name argument is undefined.");
            if (typeof attributeName !== "string") throw new Error("The attribute name argument type must be a string.");

            if (this.IsNullOrWhiteSpace(requiredLevel)) throw new Error("The required level argument is undefined.");
            if (typeof requiredLevel !== "string") throw new Error("The required level argument type must be a string.");

            let attribute = this.GetAttribute(attributeName, displayName);

            attribute.setRequiredLevel(requiredLevel);
        }

        /**
         * Sets whether data from the attribute will be submitted when the record is saved.
         * 
         * @param {!string} attributeName The attribute logical name.
         * @param {!string} submitMode The submission mode to be set to one of the following values: always, never, dirty.
         * @param {?string} [displayName] The attribute display name.
         * 
         */
        const _setAttributeSubmitMode = function (attributeName, submitMode, displayName) {

            if (this.IsNullOrWhiteSpace(attributeName)) throw new Error("The attribute name argument is undefined.");
            if (typeof attributeName !== "string") throw new Error("The attribute name argument type must be a string.");

            if (this.IsNullOrWhiteSpace(submitMode)) throw new Error("The submit mode argument is undefined.");
            if (typeof submitMode !== "string") throw new Error("The submit mode argument type must be a string.");

            let attribute = this.GetAttribute(attributeName, displayName);

            attribute.setSubmitMode(submitMode);
        }

        /**
         * Sets the value for the given attribute and returns the field instance for the particular attribute.
         * 
         * @param {!string} attributeName The attribute logical name.
         * @param {?any} attributeValue The attribute value.
         * @param {?string} [displayName] The attribute display name.
         * @returns {any}
         * 
         */
        const _setAttributeValue = function (attributeName, attributeValue, displayName) {

            if (this.IsNullOrWhiteSpace(attributeName)) throw new Error("The attribute name argument is undefined.");
            if (typeof attributeName !== "string") throw new Error("The attribute name argument type must be a string.");

            let attribute = this.GetAttribute(attributeName, displayName);

            attribute.setValue(attributeValue);

            return attribute.getValue();
        }

        /**
         * Sets the focus to the specified control.
         * 
         * @param {!string} controlName The attribute logical name.
         * @param {?string} [displayName] The attribute display name.
         *
         */
        const _setControlFocus = function (controlName, displayName) {

            if (this.IsNullOrWhiteSpace(controlName)) throw new Error("The control name argument is undefined.");
            if (typeof controlName !== "string") throw new Error("The control name argument type must be a string.");

            let control = this.GetControl(controlName, displayName);

            control.setFocus();
        }

        /**
         * Shows or hides the specified control.
         * 
         * @param {!string} controlName The attribute logical name.
         * @param {!boolean} visible The visibility state.
         * @param {?string} [displayName] The attribute display name.
         * 
         */
        const _setControlVisibility = function (controlName, visible, displayName) {

            if (this.IsNullOrWhiteSpace(controlName)) throw new Error("The control name argument is undefined.");
            if (typeof controlName !== "string") throw new Error("The control name argument type must be a string.");

            let control = this.GetControl(controlName, displayName);

            control.setVisible(visible);
        }

        /** 
         *  Makes all the form controls read-only.
         */
        const _setFormReadOnly = function () {

            let context = this;
            this.FormContext.ui.controls.forEach(function (control, index) {
                if (control.getVisible && control.getVisible()) {
                    if (control.getParent && control.getParent()) {
                        if (control.getParent().getVisible && control.getParent().getVisible()) {
                            if (control.setDisabled) context.DisableControl(control.getName(), true);
                        }
                    }
                }
            }, context);
        }

        /**
         * Sets the values of a lookup field and returns the lookup field 
         * instance for the particular attribute.
         * 
         * @param {!string} attributeName The attribute logical name.
         * @param {any} id The referenced entity primary key. 
         * @param {any} name The referenced entity primary attribute.
         * @param {any} logicalName The logical name of the referenced entity.
         * @param {any} displayName The display name of the attribute.
         * @param {?string} [displayName] The attribute display name.
         * @returns {Object} The lookup attribute value.
         * 
         */
        const _setLookupFieldValue = function (attributeName, id, name, logicalName, displayName) {

            if (this.IsNullOrWhiteSpace(attributeName)) throw new Error("The attribute name argument is undefined.");
            if (typeof attributeName !== "string") throw new Error("The attribute name argument type must be a string.");

            if (this.IsNullOrWhiteSpace(id)) throw new Error("The id argument is undefined.");
            if (typeof id !== "string") throw new Error("The id argument type must be a string.");

            if (this.IsNullOrWhiteSpace(name)) throw new Error("The name argument is undefined.");
            if (typeof name !== "string") throw new Error("The name argument type must be a string.");

            if (this.IsNullOrWhiteSpace(logicalName)) throw new Error("The logical name argument is undefined.");
            if (typeof logicalName !== "string") throw new Error("The logical name argument type must be a string.");


            let attribute = this.GetAttribute(attributeName, displayName);

            let lookup = new Array();
            lookup[0] = new Object();
            lookup[0].id = id;
            lookup[0].name = name;
            lookup[0].entityType = logicalName;
            attribute.setValue(lookup);

            return attribute.getValue();
        }

        /**
         * Set the label of the specified section under the specified tab. 
         * 
         * @param {!string} tabName The tab logical name.
         * @param {!string} sectionName The section logical name.
         * @param {!string} label The tab display name.
         * 
         */
        const _setSectionLabel = function (tabName, sectionName, label) {

            if (this.IsNullOrWhiteSpace(tabName)) throw new Error("The tab name argument is undefined.");
            if (typeof tabName !== "string") throw new Error("The tab name argument type must be a string.");

            if (this.IsNullOrWhiteSpace(sectionName)) throw new Error("The section name argument is undefined.");
            if (typeof sectionName !== "string") throw new Error("The section name argument type must be a string.");

            if (this.IsNullOrWhiteSpace(label)) throw new Error("The label argument is undefined.");
            if (typeof label !== "string") throw new Error("The label argument type must be a string.");

            let section = this.GetSection(tabName, sectionName);

            section.setLabel(label);
        }

        /**
         * Shows or hides the specified section under the specified tab.
         * 
         * @param {!string} tabName The tab logical name.
         * @param {!string} sectionName The section logical name.
         * @param {!boolean} visible The visibility state of the section.
         * 
         */
        const _setSectionVisibility = function (tabName, sectionName, visible) {

            if (this.IsNullOrWhiteSpace(tabName)) throw new Error("The tab name argument is undefined.");
            if (typeof tabName !== "string") throw new Error("The tab name argument type must be a string.");

            if (this.IsNullOrWhiteSpace(sectionName)) throw new Error("The section name argument is undefined.");
            if (typeof sectionName !== "string") throw new Error("The section name argument type must be a string.");

            if (this.IsNullOrWhiteSpace(visible)) throw new Error("The visible argument is undefined.");
            if (typeof visible !== "boolean") throw new Error("The visible argument type must be a string.");

            let tab = this.GetTab(tabName);
            let tabDisplayState = tab.getDisplayState();

            let section = this.GetSection(tabName, sectionName);

            if (visible == true && tabDisplayState == "collapsed") {
                section.setVisible(visible);
                tab.setDisplayState(tabDisplayState);
                return;
            }

            section.setVisible(visible);
        }

        /**
         * Expands or collapses the specified tab. 
         * 
         * @param {!string} tabName The tab logical name.
         * @param {!string} displayState The target display state of the tab ("expanded" or "collapsed").
         * 
         */
        const _setTabDisplayState = function (tabName, displayState) {

            if (this.IsNullOrWhiteSpace(tabName)) throw new Error("The tab name argument is undefined.");
            if (typeof tabName !== "string") throw new Error("The tab name argument type must be a string.");

            if (this.IsNullOrWhiteSpace(displayState)) throw new Error("The display state argument is undefined.");
            if (typeof displayState !== "string") throw new Error("The display state argument type must be a string.");

            let tab = this.GetTab(tabName);

            tab.setDisplayState(displayState);
        }

        /**
         * Set the label of the specified tab. 
         * 
         * @param {!string} tabName The tab logical name.
         * @param {!string} label The tab display name.
         * 
         */
        const _setTabLabel = function (tabName, label) {

            if (this.IsNullOrWhiteSpace(tabName)) throw new Error("The tab name argument is undefined.");
            if (typeof tabName !== "string") throw new Error("The tab name argument type must be a string.");

            if (this.IsNullOrWhiteSpace(label)) throw new Error("The label argument is undefined.");
            if (typeof label !== "string") throw new Error("The label argument type must be a string.");

            let tab = this.GetTab(tabName);

            tab.setLabel(label);
        }

        /**
         * Shows or hides the specified tab.
         * 
         * @param {!string} tabName The tab logical name.
         * @param {!boolean} visible The visibility state of the section.
    
         */
        const _setTabVisibility = function (tabName, visible) {

            if (this.IsNullOrWhiteSpace(tabName)) throw new Error("The tab name argument is undefined.");
            if (typeof tabName !== "string") throw new Error("The tab name argument type must be a string.");

            if (this.IsNullOrWhiteSpace(visible)) throw new Error("The visible argument is undefined.");
            if (typeof visible !== "boolean") throw new Error("The visible argument type must be a string.");

            let tab = this.GetTab(tabName);

            tab.setVisible(visible);
        }

        /**
         * Displays an error dialog containing the error details and a button.
         * 
         * @param {!string} message The error message to render.
         * @param {?string} [details] The error details.
         * @param {?Number} [errorCode] The error code.
         * @param {?Function} [successCallback] A callback reference to be called on save success.
         * @param {?Function} [errorCallback] A callback reference to be called on save error.
         *
         */
        const _showError = function (message, details, errorCode, successCallback, errorCallback) {

            if (message == null) throw new Error("The message argument is undefined.");
            if (typeof message !== "string") throw new Error("The message argument type must be a string.");

            let errorOptions = {
                message: message,
                details: details,
                errorCode: errorCode
            }

            Xrm.Navigation.openErrorDialog(errorOptions).then(successCallback, errorCallback);
        }

        /**
         * Displays an alert dialog containing a message and a button.
         * 
         * @param {!string} message The message to render.
         * @param {?string} [title] The title of the window dialog. 
         * @param {?string} [buttonLabel] The button label.
         * @param {?Function} [successCallback] A callback reference to be called on save success.
         * @param {?Function} [errorCallback] A callback reference to be called on save error.
         *
         */
        const _showMessage = function (message, title, buttonLabel, successCallback, errorCallback) {

            if (message == null) throw new Error("The message argument is undefined.");
            if (typeof message !== "string") throw new Error("The message argument type must be a string.");

            let alertStrings = {
                confirmButtonLabel: buttonLabel,
                text: message,
                title: title
            }
            let alertOptions = {
                height: 300,
                width: 500
            };

            Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then(
                function (result) {
                    if (typeof successCallback === "function") successCallback(result);
                },
                function (error) {
                    if (typeof errorCallback === "function") errorCallback(error);
                }
            );
        }

        /**
         * Handles an error by rendering the information on the user's interface.
         *
         * @param {!Error} error The actual error to process.
         * 
         */
        const _strignifyException = function (error) {

            if (typeof error === typeof undefined) throw new Error("The error argument is undefined.");
            if (typeof error !== "object") throw new Error("The error argument type must be an object.");

            let details = "";

            if (!this.IsNullOrWhiteSpace(error.fileName)) details += "Filename: " + error.fileName + "\n";
            if (!this.IsNullOrWhiteSpace(error.columnNumber)) details += "Column: " + error.columnNumber + "\n";
            if (!this.IsNullOrWhiteSpace(error.lineNumber)) details += "Line: " + error.lineNumber + "\n";
            if (!this.IsNullOrWhiteSpace(error.message)) details += "Message: " + error.message + "\n";
            if (!this.IsNullOrWhiteSpace(error.stack)) details += "Stacktrace: " + error.stack + "\n";

            window.console.log(details);

            return details;
        }

        return {
            Constants: _constants,
            FormType: _formType,
            NotificationLevel: _notificationLevel,
            SaveEventMode: _saveEventMode,

            ExecutionContext: _executionContext,
            FormContext: _formContext,
            Properties: _properties,

            ConfirmDialog: _confirmDialog,
            DateReviver: _dateReviver,
            DisableControl: _disableControl,
            FixProcessAndStage: _fixProcessAndStage,
            GetAttribute: _getAttribute,
            GetAttributeValue: _getAttributeValue,
            GetControl: _getControl,
            GetFormLabel: _getFormLabel,
            GetFormType: _getFormType,
            GetSection: _getSection,
            GetStrippedGuid: _getStrippedGuid,
            GetTab: _getTab,
            GuidsAreEqual: _guidsAreEqual,
            HandleException: _handleException,
            Initialize: _initialize, // MUST BE CALLED FIRST
            IsNull: _isNull,
            IsNullOrWhiteSpace: _isNullOrWhiteSpace,
            RefreshForm: _refreshForm,
            RefreshRibbon: _refreshRibbon,
            ReplaceText: _replaceText,
            SaveEventMode: _saveEventMode,
            SaveForm: _saveForm,
            SetAttributeControlsVisibility: _setAttributeControlsVisibility,
            SetAttributeRequired: _setAttributeRequired,
            SetAttributeSubmitMode: _setAttributeSubmitMode,
            SetAttributeValue: _setAttributeValue,
            SetControlFocus: _setControlFocus,
            SetControlVisibility: _setControlVisibility,
            SetFormReadOnly: _setFormReadOnly,
            SetLookupFieldValue: _setLookupFieldValue,
            SetSectionLabel: _setSectionLabel,
            SetSectionVisibility: _setSectionVisibility,
            SetTabDisplayState: _setTabDisplayState,
            SetTabLabel: _setTabLabel,
            SetTabVisibility: _setTabVisibility,
            ShowError: _showError,
            ShowMessage: _showMessage,
            StrignifyException: _strignifyException,
        }

    })();
}
