var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import * as _fe from "./Aylos.Xrm.Sdk.FormExtensions";
export var Aylos;
(function (Aylos) {
    let Xrm;
    (function (Xrm) {
        let Account;
        (function (Account) {
            let Forms;
            (function (Forms) {
                let Main;
                (function (Main) {
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
                    function _handleOnLoadEvent(executionContext) {
                        return __awaiter(this, void 0, void 0, function* () {
                            try {
                                let formContext = executionContext.getFormContext();
                                switch (_xrm.getFormType(formContext)) {
                                    case 1 /* Create */:
                                        _renderControls(executionContext);
                                        break;
                                    case 2 /* Update */:
                                        throw new Error(_errors.exceptions.NotImplemented);
                                    default:
                                        throw new Error(_errors.exceptions.NotImplemented);
                                }
                            }
                            catch (e) {
                                _xrm.handleException(e);
                            }
                        });
                    }
                    Main.handleOnLoadEvent = _handleOnLoadEvent;
                    /**
                     * Handles the onload event of the form.
                     *
                     * @param {Xrm.Events.EventContext} executionContext The execution context defines the event context in which your code executes.
                     *
                     */
                    function _renderControls(executionContext) {
                        try {
                            let formContext = executionContext.getFormContext();
                            console.log(formContext.ui.formSelector.getCurrentItem().getLabel());
                        }
                        catch (e) {
                            _xrm.handleException(e);
                        }
                    }
                })(Main = Forms.Main || (Forms.Main = {}));
            })(Forms = Account.Forms || (Account.Forms = {}));
        })(Account = Xrm.Account || (Xrm.Account = {}));
    })(Xrm = Aylos.Xrm || (Aylos.Xrm = {}));
})(Aylos || (Aylos = {}));
//# sourceMappingURL=Aylos.Xrm.Account.Forms.Main.js.map