import * as _fe from "./Aylos.Xrm.Sdk.FormExtensions";

export namespace Aylos {
    export namespace Xrm {
        export namespace Account {
            export namespace Forms {
                export namespace Main {
                    /**
                     * Form extensions library to be used by the Account entity main form. Please
                     * remember to minify/compress the library when releasing in CRM for improved
                     * performance. JavaScript compression removes all comments, debugger commands
                     * and replaces the variable names with shorter names.
                     */
                    const _xrm = _fe.Aylos.Xrm.Sdk.FormExtensions;
                    const _errors = _xrm.ErrorMessages;

                    /**
                     * Handles the onload event of the form.
                     * 
                     * @param {Xrm.Events.EventContext} executionContext The execution context defines the event context in which your code executes.
                     * 
                     */
                    async function _handleOnLoadEvent(executionContext: globalThis.Xrm.Events.EventContext) {

                        try {
                            let formContext = executionContext.getFormContext();
                            switch (_xrm.getFormType(formContext)) {
                                case globalThis.XrmEnum.FormType.Create:
                                    _renderControls(executionContext);
                                    break;

                                case globalThis.XrmEnum.FormType.Update:
                                    throw new Error(_errors.exceptions.NotImplemented);

                                default:
                                    throw new Error(_errors.exceptions.NotImplemented);
                            }
                        }
                        catch (e) {
                            _xrm.handleException(e);
                        }
                    }

                    export const handleOnLoadEvent = _handleOnLoadEvent;

                    /**
                     * Handles the onload event of the form.
                     *
                     * @param {Xrm.Events.EventContext} executionContext The execution context defines the event context in which your code executes.
                     *
                     */
                    function _renderControls(executionContext: globalThis.Xrm.Events.EventContext) {
                        try {
                            let formContext = executionContext.getFormContext();
                            console.log(formContext.ui.formSelector.getCurrentItem().getLabel());
                        }
                        catch (e) {
                            _xrm.handleException(e);
                        }
                    }
                }
            }
        }
    }
}