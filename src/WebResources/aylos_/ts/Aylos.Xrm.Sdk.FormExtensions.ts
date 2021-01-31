export namespace Aylos {
    export namespace Xrm {
        export namespace Sdk {
            export namespace FormExtensions {
                /**
                 * Form extensions library to be used across all modules. Please remember to
                 * minify/compress the library when releasing in the PowerPlatform environment
                 * for improved performance. JavaScript compression removes all comments, debugger
                 * commands and replaces the variable names with shorter names.
                 *
                 */

                export namespace ErrorMessages {
                    export const genericMessage: string =
                        "An error occured during execution press CTRL-C to copy the error to the "
                        + "clipboard, then create a new e-mail message and press CTRL-V to paste "
                        + "the error message into the e-mail. Then submit it to the support team.\n";
                    export enum exceptions {
                        NotImplemented = "Not implemented exception."
                    }
                }

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
                function _confirmDialog(
                    text: string,
                    title?: string | undefined,
                    subtitle?: string | undefined,
                    confirmButtonLabel?: string | undefined,
                    cancelButtonLabel?: string | undefined,
                    successCallback?: (confirm: globalThis.Xrm.Navigation.ConfirmResult) => void | null | undefined,
                    errorCallback?: (error: any) => void | null | undefined): void {

                    if (isNullOrWhiteSpace(text)) throw new Error("The text argument is undefined.");
                    if (typeof text !== "string") throw new Error("The text argument type must be a string.");

                    let confirmStrings: globalThis.Xrm.Navigation.ConfirmStrings = {
                        cancelButtonLabel: cancelButtonLabel,
                        confirmButtonLabel: confirmButtonLabel,
                        subtitle: subtitle,
                        text: text,
                        title: title
                    }

                    let confirmOptions: globalThis.Xrm.Navigation.DialogSizeOptions = {
                        height: 300,
                        width: 500
                    };

                    globalThis.Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
                        (confirm) => { if (successCallback) successCallback(confirm) },
                        (error) => { if (errorCallback) errorCallback(error) }
                    );
                }

                export const confirmDialog = _confirmDialog;

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
                function _dateReviver(value: string | null | undefined): Date | string | null | undefined {

                    if (isNullOrWhiteSpace(value) || typeof value !== "string") return value;

                    let date = Date.parse(value);
                    if (!isNaN(date)) return new Date(date);
                }

                export const dateReviver = _dateReviver;

                /**
                 * Enables or disables all related controls for a given attribute.
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object. 
                 * @param {!string} controlName The attribute logical name.
                 * @param {!boolean} disabled The attribute disabled or enabled value.
                 * @param {?string} [displayName] The attribute display name.
                 */
                function _disableControl(
                    formContext: globalThis.Xrm.FormContext,
                    controlName: string, disabled: boolean,
                    displayName?: string | null | undefined) {

                    let attribute = _getAttribute(formContext, controlName, displayName);

                    attribute.controls.forEach(function (control: globalThis.Xrm.Controls.Control) {
                        let ctrl = _getControl(formContext, control.getName(), control.getLabel());
                        ctrl.setDisabled(disabled);
                    });
                }

                export const disableControl = _disableControl;

                /**
                 * Fix the process and stage when those are not the same with the active ones.
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object. 
                 * 
                 */
                function _fixProcessAndStage(formContext: globalThis.Xrm.FormContext) {

                    let processIdAttr = _getAttribute(formContext, "processid");
                    if (_isNull(processIdAttr)) return;

                    let stageIdAttr = _getAttribute(formContext, "stageid");
                    if (_isNull(stageIdAttr)) return;

                    let activeProcess = formContext.data.process.getActiveProcess();
                    if (_isNull(activeProcess)) throw new Error("Active process is undefined.");

                    let processId = processIdAttr.getValue();
                    let stageId = stageIdAttr.getValue();
                    let activeProcessId = activeProcess.getId();

                    if (_guidsAreEqual(processId, activeProcessId)) return;

                    formContext.data.process.setActiveProcess(processId,
                        function (result) {
                            switch (result) {
                                case "success":
                                    formContext.data.process.setActiveStage(stageId);
                                    break;
                            }
                        });
                }

                export const fixProcessAndStage = _fixProcessAndStage;

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
                function _getAttribute(
                    formContext: globalThis.Xrm.FormContext,
                    attributeName: string, displayName?: string | null | undefined): globalThis.Xrm.Attributes.Attribute {

                    let label = isNullOrWhiteSpace(displayName) ? attributeName : displayName;

                    let attribute = formContext.getAttribute(attributeName);
                    if (attribute == null) throw new Error("The attribute '" + label + "' does not exist on the form.");

                    return attribute;
                }

                export const getAttribute = _getAttribute;

                /**
                 * Returns a XRM attribute value for the given attribute name. Throws an error 
                 * if the attribute does not exist on the form.
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object. 
                 * @param {!string} attributeName The attribute logical name.
                 * @param {?string} [displayName] The attribute display name.
                 *
                 */
                function _getAttributeValue(
                    formContext: globalThis.Xrm.FormContext,
                    attributeName: string, displayName?: string | null | undefined): any {

                    let attribute = _getAttribute(formContext, attributeName, displayName);

                    return attribute.getValue();
                }

                export const getAttributeValue = _getAttributeValue;

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
                function _getControl(
                    formContext: globalThis.Xrm.FormContext,
                    controlName: string, displayName?: string | null | undefined): globalThis.Xrm.Controls.StandardControl {

                    let label = isNullOrWhiteSpace(displayName) ? controlName : displayName;

                    let control = formContext.getControl<globalThis.Xrm.Controls.StandardControl>(controlName);
                    if (_isNull(control)) throw new Error("The control '" + label + "' does not exist on the form.");

                    return control;
                }

                export const getControl = _getControl;

                /**
                 *  Returns the label of the current form.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object. 
                 * 
                 * @returns {string}
                 * 
                 */
                function _getFormLabel(formContext: globalThis.Xrm.FormContext): string {

                    let currentForm = formContext.ui.formSelector.getCurrentItem();
                    if (_isNull(currentForm)) throw new Error("Current form is undefined.");

                    return currentForm.getLabel();
                }

                export const getFormLabel = _getFormLabel;

                /** 
                 * Returns the type of the form.
                 *
                 * @param {!Xrm.FormContext} formContext The form context object. 
                 *
                 */
                function _getFormType(formContext: globalThis.Xrm.FormContext): globalThis.XrmEnum.FormType {

                    return formContext.ui.getFormType();
                }

                export const getFormType = _getFormType;

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
                function _getSection(
                    formContext: globalThis.Xrm.FormContext,
                    tabName: string, sectionName: string, displayName?: string | null | undefined): globalThis.Xrm.Controls.Section {

                    let tab = _getTab(formContext, tabName);
                    if (tab == null) throw new Error("The tab '" + tabName + "' does not exist on the form.");

                    let label = isNullOrWhiteSpace(displayName) ? sectionName : displayName;

                    let section = tab.sections.get(sectionName);
                    if (section == null) throw new Error("The section '" + label + "' does not exist on the form.");

                    return section;
                }

                export const getSection = _getSection;

                /**
                 * Cleans up a Guid from spaces, curly brackets and returns a lowercase 
                 * version of it useful for comparisons.
                 * 
                 * @param {string} guid
                 */
                function _getStrippedGuid(guid: string): string {

                    return guid
                        .replace(" ", "")
                        .replace("{", "")
                        .replace("}", "")
                        .replace("[", "")
                        .replace("]", "")
                        .toLowerCase();
                }

                export const getStrippedGuid = _getStrippedGuid;

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
                function _getTab(
                    formContext: globalThis.Xrm.FormContext,
                    tabName: string, displayName?: string | null | undefined): globalThis.Xrm.Controls.Tab {

                    let label = isNullOrWhiteSpace(displayName) ? tabName : displayName;

                    let tab = formContext.ui.tabs.get<globalThis.Xrm.Controls.Tab>(tabName);
                    if (tab == null) throw new Error("The tab '" + label + "' does not exist on the form.");

                    return tab;
                }

                export const getTab = _getTab;

                /**
                 * Compares two Guids and returns true if they are equal.
                 * 
                 * @param {!string} guid1 The first GUID to compare.
                 * @param {!string} guid2 The second GUID to compare.
                 * 
                 * @returns {boolean}
                 * 
                 */
                function _guidsAreEqual(guid1: string, guid2: string): boolean {
                    return _getStrippedGuid(guid1) === _getStrippedGuid(guid2);
                }

                export const guidsAreEqual = _guidsAreEqual;

                /**
                 * Handles an error by rendering the information on the user's interface.
                 * 
                 * @param {!Error} error The actual error to process. 
                 * 
                 */
                function _handleException(error: Error): void {

                    let message = ErrorMessages.genericMessage;

                    if (!isNullOrWhiteSpace(error.message)) message = error.message;

                    let details = _strignifyException(error);

                    showError(message, details);
                }

                export const handleException = _handleException;

                /**
                  * Checks if an argument is undefined or NULL.
                  * 
                  * @param  {!any} obj the input to check.
                  * 
                  * @returns {boolean} true if satisfies the criteria.
                  * 
                  */
                function _isNull(obj: any | null | undefined): boolean {
                    return typeof obj === typeof undefined || obj === null;
                }

                export const isNull = _isNull;

                /**
                  * Checks if a string is undefined or NULL or empty or whitespace.
                  * 
                  * @param  {string} val The string to check.
                  * 
                  * @returns {boolean} true if satisfies the criteria.
                  */
                export function isNullOrWhiteSpace(val: string | null | undefined): boolean {
                    if (typeof val !== "undefined" && typeof val !== "string") return _isNull(val);
                    else if (typeof val === typeof undefined || val === null) return true;
                    else return String(val).match(/^ *$/) !== null;
                }

                /**
                 * Asynchronously refreshes and optionally saves all the data of the form without reloading the page.
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!boolean} save A boolean value to indicate if data should be saved after it is refreshed.
                 * @param {?Function} [successCallback] A callback reference to be called on save success.
                 * @param {?Function} [errorCallback] A callback reference to be called on save error.
                 * 
                 */
                export function refreshForm(
                    formContext: globalThis.Xrm.FormContext,
                    save: boolean, successCallback: Function, errorCallback: Function): void {

                    formContext.data.refresh(save).then(
                        (result) => { if (!_isNull(successCallback)) successCallback(result); },
                        (error) => { if (!_isNull(errorCallback)) errorCallback(error); }
                    );
                }

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
                export function refreshRibbon(
                    formContext: globalThis.Xrm.FormContext, refreshAll: boolean): void {

                    formContext.ui.refreshRibbon(!_isNull(refreshAll) || refreshAll);
                }

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
                export function replaceText(str: string, search: string, replace: string): string {

                    var regex = new RegExp(search, "g");
                    return str.replace(regex, replace);
                }

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
                export function saveForm(
                    formContext: globalThis.Xrm.FormContext,
                    saveOptions: globalThis.Xrm.SaveOptions, successCallback: Function, errorCallback: Function): void {

                    formContext.data.save(saveOptions).then(
                        (result) => { if (!_isNull(successCallback)) successCallback(result); },
                        (error) => { if (!_isNull(errorCallback)) errorCallback(error); }
                    );
                }

                /**
                 * Shows or hides all children controls for a given attribute.
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} attributeName The attribute logical name.
                 * @param {!boolean} visible The visibility state.
                 * @param {?string} [displayName] The attribute display name.
                 * 
                 */
                export function setAttributeControlsVisibility(
                    formContext: globalThis.Xrm.FormContext,
                    attributeName: string, visible: boolean, displayName?: string | null | undefined): void {

                    let attribute = _getAttribute(formContext, attributeName, displayName);

                    attribute.controls.forEach(function (control: globalThis.Xrm.Controls.Control) {
                        let ctrl = _getControl(formContext, control.getName(), control.getLabel());
                        ctrl.setVisible(visible);
                    });
                }

                /**
                 * Sets whether data is required, optional or recommended for the given attribute.
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} attributeName The attribute logical name.
                 * @param {!string} requirementLevel The requirement level to be set to one of the following values: none, required, recommended.
                 * @param {?string} [displayName] The attribute display name.
                 * 
                 */
                export function setAttributeRequired(
                    formContext: globalThis.Xrm.FormContext,
                    attributeName: string, requirementLevel: globalThis.XrmEnum.AttributeRequirementLevel,
                    displayName?: string | null | undefined): void {

                    let attribute = _getAttribute(formContext, attributeName, displayName);

                    attribute.setRequiredLevel(requirementLevel);
                }

                /**
                 * Sets whether data from the attribute will be submitted when the record is saved.
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} attributeName The attribute logical name.
                 * @param {!string} submitMode The submission mode to be set to one of the following values: always, never, dirty.
                 * @param {?string} [displayName] The attribute display name.
                 * 
                 */
                export function setAttributeSubmitMode(
                    formContext: globalThis.Xrm.FormContext,
                    attributeName: string, submitMode: globalThis.Xrm.SubmitMode,
                    displayName?: string | null | undefined): void {

                    let attribute = _getAttribute(formContext, attributeName, displayName);

                    attribute.setSubmitMode(submitMode);
                }

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
                export function setAttributeValue(
                    formContext: globalThis.Xrm.FormContext,
                    attributeName: string, attributeValue: any | null, displayName?: string | null | undefined): any {

                    if (isNullOrWhiteSpace(attributeName)) throw new Error("The attribute name argument is undefined.");
                    if (typeof attributeName !== "string") throw new Error("The attribute name argument type must be a string.");

                    let attribute = _getAttribute(formContext, attributeName, displayName);

                    attribute.setValue(attributeValue);

                    return attribute.getValue();
                }

                /**
                 * Sets the focus to the specified control.
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} controlName The attribute logical name.
                 * @param {?string} [displayName] The attribute display name.
                 *
                 */
                export function setControlFocus(
                    formContext: globalThis.Xrm.FormContext,
                    controlName: string, displayName?: string | null | undefined): void {

                    let control = _getControl(formContext, controlName, displayName);

                    control.setFocus();
                }

                /**
                 * Shows or hides the specified control.
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} controlName The attribute logical name.
                 * @param {!boolean} visible The visibility state.
                 * @param {?string} [displayName] The attribute display name.
                 * 
                 */
                export function setControlVisibility(
                    formContext: globalThis.Xrm.FormContext,
                    controlName: string, visible: boolean, displayName?: string | null | undefined): void {

                    let control = _getControl(formContext, controlName, displayName);

                    control.setVisible(visible);
                }

                /**
                 * TO-DO!!!
                 * 
                 * @param control
                 */
                export function isControlVisible(control: globalThis.Xrm.Controls.Control | globalThis.Xrm.Controls.Section | globalThis.Xrm.Controls.Tab | any): boolean {

                    if (control.getVisible && control.getVisible()) {
                        if (control.getParent && control.getParent()) {
                            return isControlVisible(control.getParent());
                        }
                        return true;
                    }
                    return false;
                }

                /** 
                 *  Makes all the form controls read-only.
                 *  
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * 
                 */
                export function setFormReadOnly(formContext: globalThis.Xrm.FormContext): void {

                    formContext.ui.controls.forEach(function (control) {
                        if (isControlVisible(control)) {
                            if ("standard,lookup,optionset,multiselectoptionset".includes(control.getControlType())) {
                                let ctrl = formContext.getControl<globalThis.Xrm.Controls.StandardControl>(control.getName());
                                let attribute = ctrl.getAttribute();
                                if (attribute != null) {
                                    setAttributeSubmitMode(formContext, control.getName(), "never");
                                    _disableControl(formContext, control.getName(), true);
                                }
                            }
                        }
                    });
                }

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
                export function setLookupFieldValue(
                    formContext: globalThis.Xrm.FormContext,
                    attributeName: string, id: string, name: string, logicalName: string,
                    displayName?: string | null | undefined): any {

                    let lookup = new Array();
                    lookup[0] = new Object();
                    lookup[0].id = id;
                    lookup[0].name = name;
                    lookup[0].entityType = logicalName;

                    let attribute = _getAttribute(formContext, attributeName, displayName);
                    attribute.setValue(lookup);

                    return attribute.getValue();
                }

                /**
                 * Set the label of the specified section under the specified tab. 
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} tabName The tab logical name.
                 * @param {!string} sectionName The section logical name.
                 * @param {!string} label The tab display name.
                 * 
                 */
                export function setSectionLabel(
                    formContext: globalThis.Xrm.FormContext,
                    tabName: string, sectionName: string, label: string): void {

                    let section = _getSection(formContext, tabName, sectionName);
                    section.setLabel(label);
                }

                /**
                 * Shows or hides the specified section under the specified tab.
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} tabName The tab logical name.
                 * @param {!string} sectionName The section logical name.
                 * @param {!boolean} visible The visibility state of the section.
                 * 
                 */
                export function setSectionVisibility(
                    formContext: globalThis.Xrm.FormContext,
                    tabName: string, sectionName: string, visible: boolean): void {

                    let tab = _getTab(formContext, tabName);
                    let tabDisplayState = tab.getDisplayState();
                    if (visible === true && tabDisplayState === "collapsed") {
                        tab.setDisplayState(tabDisplayState);
                    }

                    let section = _getSection(formContext, tabName, sectionName);
                    section.setVisible(visible);
                }

                /**
                 * Expands or collapses the specified tab. 
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} tabName The tab logical name.
                 * @param {!Xrm.DisplayState} displayState The target display state of the tab ("expanded" or "collapsed").
                 * 
                 */
                export function setTabDisplayState(
                    formContext: globalThis.Xrm.FormContext,
                    tabName: string, displayState: globalThis.Xrm.DisplayState): void {

                    let tab = _getTab(formContext, tabName);
                    tab.setDisplayState(displayState);
                }

                /**
                 * Set the label of the specified tab. 
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} tabName The tab logical name.
                 * @param {!string} label The tab display name.
                 * 
                 */
                export function setTabLabel(
                    formContext: globalThis.Xrm.FormContext,
                    tabName: string, label: string): void {

                    let tab = _getTab(formContext, tabName);
                    tab.setLabel(label);
                }

                /**
                 * Shows or hides the specified tab.
                 * 
                 * @param {!Xrm.FormContext} formContext The form context object.
                 * @param {!string} tabName The tab logical name.
                 * @param {!boolean} visible The visibility state of the section.
                 * 
                 */
                export function setTabVisibility(
                    formContext: globalThis.Xrm.FormContext,
                    tabName: string, visible: boolean): void {

                    let tab = _getTab(formContext, tabName);
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
                export function showError(
                    message: string, details?: string | undefined, errorCode?: number | undefined,
                    successCallback?: Function | undefined, errorCallback?: Function | undefined): void {

                    let errorOptions: globalThis.Xrm.Navigation.ErrorDialogOptions = {
                        message: message,
                        details: details,
                        errorCode: errorCode
                    }

                    globalThis.Xrm.Navigation.openErrorDialog(errorOptions).then(
                        (result) => { if (successCallback) successCallback(result); },
                        (error) => { if (errorCallback) errorCallback(error); }
                    );
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
                function _showMessage(
                    message: string, title?: string, buttonLabel?: string,
                    successCallback?: Function | undefined, errorCallback?: Function | undefined): void {

                    let alertStrings = {
                        confirmButtonLabel: buttonLabel,
                        text: message,
                        title: title
                    }
                    let alertOptions = {
                        height: 300,
                        width: 500
                    };

                    globalThis.Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then(
                        (result) => { if (successCallback) successCallback(result); },
                        (error) => { if (errorCallback) errorCallback(error); }
                    );
                }

                export const showMessage = _showMessage;

                /**
                 * Handles an error by rendering the information on the user's interface.
                 *
                 * @param {!Error} error The actual error to process.
                 * 
                 */
                function _strignifyException(error: object | any): string {

                    let details = "";

                    if (!isNullOrWhiteSpace(error.fileName)) details += "Filename: " + error.fileName + "\n";
                    if (!isNullOrWhiteSpace(error.columnNumber)) details += "Column: " + error.columnNumber + "\n";
                    if (!isNullOrWhiteSpace(error.lineNumber)) details += "Line: " + error.lineNumber + "\n";
                    if (!isNullOrWhiteSpace(error.message)) details += "Message: " + error.message + "\n";
                    if (!isNullOrWhiteSpace(error.stack)) details += "Stacktrace: " + error.stack + "\n";

                    console.log(details);

                    return details;
                }

                export const strignifyException = _strignifyException;
            }
        }
    }
}

