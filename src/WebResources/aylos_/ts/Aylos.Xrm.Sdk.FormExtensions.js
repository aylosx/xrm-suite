export var Aylos;
(function (Aylos) {
    let Xrm;
    (function (Xrm) {
        let Sdk;
        (function (Sdk) {
            let FormExtensions;
            (function (FormExtensions) {
                /**
                 * Form extensions library to be used across all modules. Please remember to
                 * minify/compress the library when releasing in the PowerPlatform environment
                 * for improved performance. JavaScript compression removes all comments, debugger
                 * commands and replaces the variable names with shorter names.
                 *
                 */
                let ErrorMessages;
                (function (ErrorMessages) {
                    ErrorMessages.genericMessage = "An error occured during execution press CTRL-C to copy the error to the "
                        + "clipboard, then create a new e-mail message and press CTRL-V to paste "
                        + "the error message into the e-mail. Then submit it to the support team.\n";
                    let exceptions;
                    (function (exceptions) {
                        exceptions["NotImplemented"] = "Not implemented exception.";
                    })(exceptions = ErrorMessages.exceptions || (ErrorMessages.exceptions = {}));
                })(ErrorMessages = FormExtensions.ErrorMessages || (FormExtensions.ErrorMessages = {}));
                /**
                 * Displays a confirmation dialog containing a message and buttons.
                 *
                 * @param {!string} text The text to render.
                 * @param {?string} [title] The title of the window dialog.
                 * @param {?string} [subtitle] The sub title of the window dialog.
                 * @param {?string} [confirmButtonLabel] The confirm button label.
                 * @param {?string} [cancelButtonLabel] The cancel button label.
                 * @param {?Xrm.Navigation.ConfirmResult} [successCallback] A callback reference to be called on save success.
                 * @param {?Function} [errorCallback] A callback reference to be called on save error.
                 *
                 */
                function _confirmDialog(text, title, subtitle, confirmButtonLabel, cancelButtonLabel, successCallback, errorCallback) {
                    if (isNullOrWhiteSpace(text))
                        throw new Error("The text argument is undefined.");
                    if (typeof text !== "string")
                        throw new Error("The text argument type must be a string.");
                    let confirmStrings = {
                        cancelButtonLabel: cancelButtonLabel,
                        confirmButtonLabel: confirmButtonLabel,
                        subtitle: subtitle,
                        text: text,
                        title: title
                    };
                    let confirmOptions = {
                        height: 300,
                        width: 500
                    };
                    globalThis.Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then((confirm) => { if (successCallback)
                        successCallback(confirm); }, (error) => { if (errorCallback)
                        errorCallback(error); });
                }
                FormExtensions.confirmDialog = _confirmDialog;
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
                function _dateReviver(value) {
                    if (isNullOrWhiteSpace(value) || typeof value !== "string")
                        return value;
                    let date = Date.parse(value);
                    if (!isNaN(date))
                        return new Date(date);
                }
                FormExtensions.dateReviver = _dateReviver;
                /**
                 * Enables or disables all related controls for a given attribute.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} controlName The attribute logical name.
                 * @param {!boolean} disabled The attribute disabled or enabled value.
                 * @param {?string} [displayName] The attribute display name.
                 */
                function _disableControl(formContext, controlName, disabled, displayName) {
                    let attribute = _getAttribute(formContext, controlName, displayName);
                    attribute.controls.forEach(function (control) {
                        let ctrl = _getControl(formContext, control.getName(), control.getLabel());
                        ctrl.setDisabled(disabled);
                    });
                }
                FormExtensions.disableControl = _disableControl;
                /**
                 * Fix the process and stage when those are not the same with the active ones.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 *
                 */
                function _fixProcessAndStage(formContext) {
                    let processIdAttr = _getAttribute(formContext, "processid");
                    if (_isNull(processIdAttr))
                        return;
                    let stageIdAttr = _getAttribute(formContext, "stageid");
                    if (_isNull(stageIdAttr))
                        return;
                    let activeProcess = formContext.data.process.getActiveProcess();
                    if (_isNull(activeProcess))
                        throw new Error("Active process is undefined.");
                    let processId = processIdAttr.getValue();
                    let stageId = stageIdAttr.getValue();
                    let activeProcessId = activeProcess.getId();
                    if (_guidsAreEqual(processId, activeProcessId))
                        return;
                    formContext.data.process.setActiveProcess(processId, function (result) {
                        switch (result) {
                            case "success":
                                formContext.data.process.setActiveStage(stageId);
                                break;
                        }
                    });
                }
                FormExtensions.fixProcessAndStage = _fixProcessAndStage;
                /**
                 * Returns an attribute instance for the given attribute name. Throws an error
                 * if the attribute does not exist on the form.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} attributeName The attribute logical name.
                 * @param {?string} [displayName] The attribute display name.
                 *
                 * @returns {Xrm.Attributes.Attribute}
                 *
                 */
                function _getAttribute(formContext, attributeName, displayName) {
                    let label = isNullOrWhiteSpace(displayName) ? attributeName : displayName;
                    let attribute = formContext.getAttribute(attributeName);
                    if (attribute == null)
                        throw new Error("The attribute '" + label + "' does not exist on the form.");
                    return attribute;
                }
                FormExtensions.getAttribute = _getAttribute;
                /**
                 * Returns a XRM attribute value for the given attribute name. Throws an error
                 * if the attribute does not exist on the form.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} attributeName The attribute logical name.
                 * @param {?string} [displayName] The attribute display name.
                 *
                 */
                function _getAttributeValue(formContext, attributeName, displayName) {
                    let attribute = _getAttribute(formContext, attributeName, displayName);
                    return attribute.getValue();
                }
                FormExtensions.getAttributeValue = _getAttributeValue;
                /**
                 * Returns a XRM form control. Throws an error
                 * if the control does not exist on the form.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} controlName The control name.
                 * @param {?string} [displayName] The control display name.
                 *
                 * @returns {Xrm.Controls.StandardControl}
                 *
                 */
                function _getControl(formContext, controlName, displayName) {
                    let label = isNullOrWhiteSpace(displayName) ? controlName : displayName;
                    let control = formContext.getControl(controlName);
                    if (_isNull(control))
                        throw new Error("The control '" + label + "' does not exist on the form.");
                    return control;
                }
                FormExtensions.getControl = _getControl;
                /**
                 *  Returns the label of the current form.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 *
                 * @returns {string}
                 *
                 */
                function _getFormLabel(formContext) {
                    let currentForm = formContext.ui.formSelector.getCurrentItem();
                    if (_isNull(currentForm))
                        throw new Error("Current form is undefined.");
                    return currentForm.getLabel();
                }
                FormExtensions.getFormLabel = _getFormLabel;
                /**
                 * Returns the type of the form.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 *
                 */
                function _getFormType(formContext) {
                    return formContext.ui.getFormType();
                }
                FormExtensions.getFormType = _getFormType;
                /**
                 * Returns a section instance for the given tab and section name. Throws an error
                 * if the tab or the section does not exist on the form.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} tabName The tab logical name.
                 * @param {!string} sectionName The tab logical name.
                 * @param {?string} [displayName] The tab display name.
                 *
                 * @returns {Xrm.Controls.Section} The section object.
                 *
                 */
                function _getSection(formContext, tabName, sectionName, displayName) {
                    let tab = _getTab(formContext, tabName);
                    if (tab == null)
                        throw new Error("The tab '" + tabName + "' does not exist on the form.");
                    let label = isNullOrWhiteSpace(displayName) ? sectionName : displayName;
                    let section = tab.sections.get(sectionName);
                    if (section == null)
                        throw new Error("The section '" + label + "' does not exist on the form.");
                    return section;
                }
                FormExtensions.getSection = _getSection;
                /**
                 * Cleans up a Guid from spaces, curly brackets and returns a lowercase
                 * version of it useful for comparisons.
                 *
                 * @param {string} guid
                 */
                function _getStrippedGuid(guid) {
                    return guid
                        .replace(" ", "")
                        .replace("{", "")
                        .replace("}", "")
                        .replace("[", "")
                        .replace("]", "")
                        .toLowerCase();
                }
                FormExtensions.getStrippedGuid = _getStrippedGuid;
                /**
                 * Returns a tab instance for the given tab name. Throws an error
                 * if the tab does not exist on the form.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} tabName The tab logical name.
                 * @param {?string} [displayName] The tab display name.
                 *
                 * @returns {Xrm.Controls.Tab} The tab object.
                 *
                 */
                function _getTab(formContext, tabName, displayName) {
                    let label = isNullOrWhiteSpace(displayName) ? tabName : displayName;
                    let tab = formContext.ui.tabs.get(tabName);
                    if (tab == null)
                        throw new Error("The tab '" + label + "' does not exist on the form.");
                    return tab;
                }
                FormExtensions.getTab = _getTab;
                /**
                 * Compares two Guids and returns true if they are equal.
                 *
                 * @param {!string} guid1 The first GUID to compare.
                 * @param {!string} guid2 The second GUID to compare.
                 *
                 * @returns {boolean}
                 *
                 */
                function _guidsAreEqual(guid1, guid2) {
                    return _getStrippedGuid(guid1) === _getStrippedGuid(guid2);
                }
                FormExtensions.guidsAreEqual = _guidsAreEqual;
                /**
                 * Handles an error by rendering the information on the user's interface.
                 *
                 * @param {!Error} error The actual error to process.
                 *
                 */
                function _handleException(error) {
                    let message = ErrorMessages.genericMessage;
                    if (!isNullOrWhiteSpace(error.message))
                        message = error.message;
                    let details = _strignifyException(error);
                    showError(message, details);
                }
                FormExtensions.handleException = _handleException;
                /**
                  * Checks if an argument is undefined or NULL.
                  *
                  * @param  {!any} obj the input to check.
                  *
                  * @returns {boolean} true if satisfies the criteria.
                  *
                  */
                function _isNull(obj) {
                    return typeof obj === typeof undefined || obj === null;
                }
                FormExtensions.isNull = _isNull;
                /**
                  * Checks if a string is undefined or NULL or empty or whitespace.
                  *
                  * @param  {string} val The string to check.
                  *
                  * @returns {boolean} true if satisfies the criteria.
                  */
                function isNullOrWhiteSpace(val) {
                    if (typeof val !== "undefined" && typeof val !== "string")
                        return _isNull(val);
                    else if (typeof val === typeof undefined || val === null)
                        return true;
                    else
                        return String(val).match(/^ *$/) !== null;
                }
                FormExtensions.isNullOrWhiteSpace = isNullOrWhiteSpace;
                /**
                 * Asynchronously refreshes and optionally saves all the data of the form without reloading the page.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!boolean} save A boolean value to indicate if data should be saved after it is refreshed.
                 * @param {?Function} [successCallback] A callback reference to be called on save success.
                 * @param {?Function} [errorCallback] A callback reference to be called on save error.
                 *
                 */
                function refreshForm(formContext, save, successCallback, errorCallback) {
                    formContext.data.refresh(save).then((result) => { if (!_isNull(successCallback))
                        successCallback(result); }, (error) => { if (!_isNull(errorCallback))
                        errorCallback(error); });
                }
                FormExtensions.refreshForm = refreshForm;
                /**
                 * Causes the ribbon to re-evaluate data that controls what is displayed in it. Triggers a refresh
                 * ribbon action when data that the ribbon depends get changed. Call the function in the code or
                 * as part of the on-change event of a particular field.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {?boolean} [refreshAll] Indicates whether all the ribbon command bars on the current page are refreshed.
                 * If you specify false, only the page-level ribbon command bar is refreshed. If you do not specify FormExtensions
                 * parameter, by default false is passed.
                 *
                 */
                function refreshRibbon(formContext, refreshAll) {
                    formContext.ui.refreshRibbon(!_isNull(refreshAll) || refreshAll);
                }
                FormExtensions.refreshRibbon = refreshRibbon;
                /**
                 * Replaces all instances of the search text in the specified string
                 * with the replace text.
                 *
                 * @param {any} str The specified string to modify.
                 * @param {any} search The text to match and replace in the given string.
                 * @param {any} replace The text to use as a replacement in the given string.
                 *
                 * @returns {string} The string result.
                 *
                 * https://regex101.com/codegen?language=javascript
                 *
                 */
                function replaceText(str, search, replace) {
                    var regex = new RegExp(search, "g");
                    return str.replace(regex, replace);
                }
                FormExtensions.replaceText = replaceText;
                /**
                 * Saves the record asynchronously with the option to set callback functions to be executed after the save operation is completed.
                 * You can also set an object to control how appointment, recurring appointment, or service activity records are processed.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
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
                function saveForm(formContext, saveOptions, successCallback, errorCallback) {
                    formContext.data.save(saveOptions).then((result) => { if (!_isNull(successCallback))
                        successCallback(result); }, (error) => { if (!_isNull(errorCallback))
                        errorCallback(error); });
                }
                FormExtensions.saveForm = saveForm;
                /**
                 * Shows or hides all children controls for a given attribute.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} attributeName The attribute logical name.
                 * @param {!boolean} visible The visibility state.
                 * @param {?string} [displayName] The attribute display name.
                 *
                 */
                function setAttributeControlsVisibility(formContext, attributeName, visible, displayName) {
                    let attribute = _getAttribute(formContext, attributeName, displayName);
                    attribute.controls.forEach(function (control) {
                        let ctrl = _getControl(formContext, control.getName(), control.getLabel());
                        ctrl.setVisible(visible);
                    });
                }
                FormExtensions.setAttributeControlsVisibility = setAttributeControlsVisibility;
                /**
                 * Sets whether data is required, optional or recommended for the given attribute.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} attributeName The attribute logical name.
                 * @param {!string} requirementLevel The requirement level to be set to one of the following values: none, required, recommended.
                 * @param {?string} [displayName] The attribute display name.
                 *
                 */
                function setAttributeRequired(formContext, attributeName, requirementLevel, displayName) {
                    let attribute = _getAttribute(formContext, attributeName, displayName);
                    attribute.setRequiredLevel(requirementLevel);
                }
                FormExtensions.setAttributeRequired = setAttributeRequired;
                /**
                 * Sets whether data from the attribute will be submitted when the record is saved.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} attributeName The attribute logical name.
                 * @param {!string} submitMode The submission mode to be set to one of the following values: always, never, dirty.
                 * @param {?string} [displayName] The attribute display name.
                 *
                 */
                function setAttributeSubmitMode(formContext, attributeName, submitMode, displayName) {
                    let attribute = _getAttribute(formContext, attributeName, displayName);
                    attribute.setSubmitMode(submitMode);
                }
                FormExtensions.setAttributeSubmitMode = setAttributeSubmitMode;
                /**
                 * Sets the value for the given attribute and returns the field instance for the particular attribute.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} attributeName The attribute logical name.
                 * @param {?any} attributeValue The attribute value.
                 * @param {?string} [displayName] The attribute display name.
                 *
                 * @returns {any}
                 *
                 */
                function setAttributeValue(formContext, attributeName, attributeValue, displayName) {
                    if (isNullOrWhiteSpace(attributeName))
                        throw new Error("The attribute name argument is undefined.");
                    if (typeof attributeName !== "string")
                        throw new Error("The attribute name argument type must be a string.");
                    let attribute = _getAttribute(formContext, attributeName, displayName);
                    attribute.setValue(attributeValue);
                    return attribute.getValue();
                }
                FormExtensions.setAttributeValue = setAttributeValue;
                /**
                 * Sets the focus to the specified control.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} controlName The attribute logical name.
                 * @param {?string} [displayName] The attribute display name.
                 *
                 */
                function setControlFocus(formContext, controlName, displayName) {
                    let control = _getControl(formContext, controlName, displayName);
                    control.setFocus();
                }
                FormExtensions.setControlFocus = setControlFocus;
                /**
                 * Shows or hides the specified control.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} controlName The attribute logical name.
                 * @param {!boolean} visible The visibility state.
                 * @param {?string} [displayName] The attribute display name.
                 *
                 */
                function setControlVisibility(formContext, controlName, visible, displayName) {
                    let control = _getControl(formContext, controlName, displayName);
                    control.setVisible(visible);
                }
                FormExtensions.setControlVisibility = setControlVisibility;
                /**
                 * TO-DO!!!
                 *
                 * @param control
                 */
                function isControlVisible(control) {
                    if (control.getVisible && control.getVisible()) {
                        if (control.getParent && control.getParent()) {
                            return isControlVisible(control.getParent());
                        }
                        return true;
                    }
                    return false;
                }
                FormExtensions.isControlVisible = isControlVisible;
                /**
                 *  Makes all the form controls read-only.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 *
                 */
                function setFormReadOnly(formContext) {
                    formContext.ui.controls.forEach(function (control) {
                        if (isControlVisible(control)) {
                            if ("standard,lookup,optionset,multiselectoptionset".includes(control.getControlType())) {
                                let ctrl = formContext.getControl(control.getName());
                                let attribute = ctrl.getAttribute();
                                if (attribute != null) {
                                    setAttributeSubmitMode(formContext, control.getName(), "never");
                                    _disableControl(formContext, control.getName(), true);
                                }
                            }
                        }
                    });
                }
                FormExtensions.setFormReadOnly = setFormReadOnly;
                /**
                 * Sets the values of a lookup field and returns the lookup field
                 * instance for the particular attribute.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} attributeName The attribute logical name.
                 * @param {!string} id The referenced entity primary key.
                 * @param {!string} name The referenced entity primary attribute.
                 * @param {!string} logicalName The logical name of the referenced entity.
                 * @param {?string} [displayName] The attribute display name.

                  * @returns {any} The lookup attribute value.
                 *
                 */
                function setLookupFieldValue(formContext, attributeName, id, name, logicalName, displayName) {
                    let lookup = new Array();
                    lookup[0] = new Object();
                    lookup[0].id = id;
                    lookup[0].name = name;
                    lookup[0].entityType = logicalName;
                    let attribute = _getAttribute(formContext, attributeName, displayName);
                    attribute.setValue(lookup);
                    return attribute.getValue();
                }
                FormExtensions.setLookupFieldValue = setLookupFieldValue;
                /**
                 * Set the label of the specified section under the specified tab.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} tabName The tab logical name.
                 * @param {!string} sectionName The section logical name.
                 * @param {!string} label The tab display name.
                 *
                 */
                function setSectionLabel(formContext, tabName, sectionName, label) {
                    let section = _getSection(formContext, tabName, sectionName);
                    section.setLabel(label);
                }
                FormExtensions.setSectionLabel = setSectionLabel;
                /**
                 * Shows or hides the specified section under the specified tab.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} tabName The tab logical name.
                 * @param {!string} sectionName The section logical name.
                 * @param {!boolean} visible The visibility state of the section.
                 *
                 */
                function setSectionVisibility(formContext, tabName, sectionName, visible) {
                    let tab = _getTab(formContext, tabName);
                    let tabDisplayState = tab.getDisplayState();
                    if (visible === true && tabDisplayState === "collapsed") {
                        tab.setDisplayState(tabDisplayState);
                    }
                    let section = _getSection(formContext, tabName, sectionName);
                    section.setVisible(visible);
                }
                FormExtensions.setSectionVisibility = setSectionVisibility;
                /**
                 * Expands or collapses the specified tab.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} tabName The tab logical name.
                 * @param {!Xrm.DisplayState} displayState The target display state of the tab ("expanded" or "collapsed").
                 *
                 */
                function setTabDisplayState(formContext, tabName, displayState) {
                    let tab = _getTab(formContext, tabName);
                    tab.setDisplayState(displayState);
                }
                FormExtensions.setTabDisplayState = setTabDisplayState;
                /**
                 * Set the label of the specified tab.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} tabName The tab logical name.
                 * @param {!string} label The tab display name.
                 *
                 */
                function setTabLabel(formContext, tabName, label) {
                    let tab = _getTab(formContext, tabName);
                    tab.setLabel(label);
                }
                FormExtensions.setTabLabel = setTabLabel;
                /**
                 * Shows or hides the specified tab.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} tabName The tab logical name.
                 * @param {!boolean} visible The visibility state of the section.
                 *
                 */
                function setTabVisibility(formContext, tabName, visible) {
                    let tab = _getTab(formContext, tabName);
                    tab.setVisible(visible);
                }
                FormExtensions.setTabVisibility = setTabVisibility;
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
                function showError(message, details, errorCode, successCallback, errorCallback) {
                    let errorOptions = {
                        message: message,
                        details: details,
                        errorCode: errorCode
                    };
                    globalThis.Xrm.Navigation.openErrorDialog(errorOptions).then((result) => { if (successCallback)
                        successCallback(result); }, (error) => { if (errorCallback)
                        errorCallback(error); });
                }
                FormExtensions.showError = showError;
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
                function _showMessage(message, title, buttonLabel, successCallback, errorCallback) {
                    let alertStrings = {
                        confirmButtonLabel: buttonLabel,
                        text: message,
                        title: title
                    };
                    let alertOptions = {
                        height: 300,
                        width: 500
                    };
                    globalThis.Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then((result) => { if (successCallback)
                        successCallback(result); }, (error) => { if (errorCallback)
                        errorCallback(error); });
                }
                FormExtensions.showMessage = _showMessage;
                /**
                 * Handles an error by rendering the information on the user's interface.
                 *
                 * @param {!Error} error The actual error to process.
                 *
                 */
                function _strignifyException(error) {
                    let details = "";
                    if (!isNullOrWhiteSpace(error.fileName))
                        details += "Filename: " + error.fileName + "\n";
                    if (!isNullOrWhiteSpace(error.columnNumber))
                        details += "Column: " + error.columnNumber + "\n";
                    if (!isNullOrWhiteSpace(error.lineNumber))
                        details += "Line: " + error.lineNumber + "\n";
                    if (!isNullOrWhiteSpace(error.message))
                        details += "Message: " + error.message + "\n";
                    if (!isNullOrWhiteSpace(error.stack))
                        details += "Stacktrace: " + error.stack + "\n";
                    console.log(details);
                    return details;
                }
                FormExtensions.strignifyException = _strignifyException;
            })(FormExtensions = Sdk.FormExtensions || (Sdk.FormExtensions = {}));
        })(Sdk = Xrm.Sdk || (Xrm.Sdk = {}));
    })(Xrm = Aylos.Xrm || (Aylos.Xrm = {}));
})(Aylos || (Aylos = {}));
//# sourceMappingURL=Aylos.Xrm.Sdk.FormExtensions.js.map