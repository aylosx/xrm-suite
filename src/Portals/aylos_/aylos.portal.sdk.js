if (typeof Aylos === "undefined") { Aylos = { __namespace: true } }
if (typeof Aylos.Portal === "undefined") { Aylos.Portal = { __namespace: true } }
if (typeof Aylos.Portal.AdvancedForms === "undefined") { Aylos.Portal.AdvancedForms = { __namespace: true } }
if (typeof Aylos.Portal.DomHelpers === "undefined") { Aylos.Portal.DomHelpers = { __namespace: true } }
if (typeof Aylos.Portal.Network === "undefined") { Aylos.Portal.Network = { __namespace: true } }
if (typeof Aylos.Portal.Payments === "undefined") { Aylos.Portal.Payments = { __namespace: true } }
if (typeof Aylos.Portal.Storage === "undefined") { Aylos.Portal.Storage = { __namespace: true } }

(function () {
    /**
     * Aylos.Portal
     */

    "use strict";

}).apply(Aylos.Portal);

(function () {
    /**
     * Aylos.Portal.AdvancedForms
     */

    "use strict";

    /**
      * Checks if a string is NULL or empty or contains whitespace only.
      * @param  {string} str the string to check.
      * @returns {Boolean} true if satisfies the criteria.
      */
    const _isNullOrWhiteSpace = function (str) {

        return typeof (str) === "undefined" || str === null || str.match(/^ *$/) !== null;
    };

    /**
      * Returns the index of the given step if it is a return step.
      * @param {string} stepId the step id to check.
      * @returns {integer}
      */
    const _getReturnStep = function (stepId) {

        try {
            if (_isNullOrWhiteSpace(stepId)) return false;
            if (_config.payment == null || _config.payment.returnStep == null) return false;
            return stepId === _config.payment.returnStep.id ? _config.payment.returnStep.index : null;
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
      * Returns the object if not a string or NULL if a 
      * string is NULL or empty or contains whitespace only.
      * @param  {string} obj the string to check.
      * @returns {object/string} object/null/string
      */
    const _nullify = function (obj) {

        if (obj == null || typeof obj !== "string") return obj;
        return obj === null || obj.match(/^ *$/) !== null ? null : obj;
    };

    /**
     * Retrieves input data from a formObject and returns it as a JSON object.
     * @param {HTMLFormControlsCollection} elements the formObject elements.
     * @returns {Object} data as an object literal.
     */
    const _formToJSON = function (elements) {

        try {
            // Ensure no error is pending
            if (_error.occured) return;

            if (elements == null) throw new Error("The elements argument is null or undefined.");
            if (typeof elements !== "function") throw new Error("The obj argument type must be elements.");

            let entityNameElement = _apd.GetElement(_entityNameFieldName);
            _session.current.entityName = entityNameElement == null ? "" : entityNameElement.value;

            let prefix = entityNameElement && entityNameElement.value ? entityNameElement.value + "#" : "";
            return [].reduce.call(elements, function (data, element) {
                /*
                 * Check if the element contained in the exclusion list.
                 */
                let excludedElement = _excludedFieldIds.indexOf(element.id) > -1
                if (!_apd.IsButton(element) && !excludedElement && _apd.IsValidElement(element) && _apd.IsValidValue(element)) {
                    /*
                     * Some fields allow for more than one value, so we need to check if this
                     * is one of those fields and, if so, store the values as an array.
                     */
                    if (_apd.IsCheckbox(element)) {
                        data[prefix + element.id] = _nullify((data[prefix + element.id] || []).concat(element.value));
                    } else if (_apd.IsMultiSelect(element)) {
                        data[prefix + element.id] = _nullify(_apd.GetSelectValues(element));
                    } else {
                        data[prefix + element.id] = _nullify(element.value);
                        if (element.id === _entityIdFieldName) _session.current.entityId = element.value;
                        if (prefix + element.id === _config.behaviour.serviceReferenceNumberMapping) _session.config.serviceReferenceNumber = element.value;
                        if (prefix + element.id === _config.behaviour.primaryEntityId) _session.config.primaryEntityId = element.value;
                    }
                    /*
                     * Set the particular session value according to the payment field mapping.
                     */
                    let arr = _apd.GetObjectByValue(_config.payment.fieldMapping, prefix + element.id);
                    if (arr != null) {
                        for (let key in _session.payment.details) {
                            if (key === arr[0]) {
                                _session.payment.details[key] = _nullify(data[prefix + element.id]);
                                break;
                            }
                        }
                    }
                }
                // The result is then returned to the caller.
                return data;
            }, {});
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
     * Clears storage and redirects to the error page.
     * @param {string} errorMessage 
     * @returns {void}
     */
    const _handleError = function (error) {

        if (typeof (error) === undefined) {
            window.console.log("An undefined error occurred.");
            // Set local error
            _error.message = "undefined";
            _error.error = null;
            _error.occured = true;
        }
        else {
            if (!_isNullOrWhiteSpace(error.name)) window.console.log(error.name);
            if (!_isNullOrWhiteSpace(error.message)) window.console.log(error.message);
            if (!_isNullOrWhiteSpace(error.description)) window.console.log(error.description);
            if (!_isNullOrWhiteSpace(error.stack)) window.console.log(error.stack);
            // Set local error
            _error.message = error.message;
            _error.error = error;
            _error.occured = true;
        }
        // Redirect to the error page
        window.location.href = window.location.origin + _routes.errorRedirect;
    };

    /**
     * Handles the error redirect step.
     * @returns {void}
     */
    const _handleErrorRedirectStep = function () {

        //console.log("_handleErrorRedirectStep");
        try {
            // Ensure no error is pending
            if (_error.occured) return;

            // Close session
            _closeSession();

            // Initialize the return home button
            $('#' + _buttons.returnHomeButton).val(_defaultReturnHomeButtonLabel);
            $('#' + _buttons.returnHomeButton).click(function () {
                // Redirect to the referrer web site
                window.location.href = _defaultOriginUrl;
            });
        }
        catch (e) {
            //console.log(e.message);
        }
    };

    /**
     * Handles the payment accepted step.
     * @returns {void}
     */
    const _handlePaymentAcceptedStep = function () {

        //console.log("_handlePaymentAcceptedStep");
        try {
            // Ensure no error is pending
            if (_error.occured) return;

            // Close session
            _closeSession();

            // Initialize the return home button
            $('#' + _buttons.returnHomeButton).val(_session.config.returnHomeButtonLabel);
            $('#' + _buttons.returnHomeButton).click(function () {
                // Redirect to the referrer web site
                window.location.href = _session.config.originUrl;
            });
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
     * Handles the payment cancelled step.
     * @returns {void}
     */
    const _handlePaymentCancelledStep = function () {

        //console.log("_handlePaymentCancelledStep");
        try {
            // Ensure no error is pending
            if (_error.occured) return;

            // Initialize the cancel button
            $('#' + _buttons.cancelButton).click(function () {
                // Redirect to the referrer web site
                window.location.href = _session.config.originUrl;
            });

            // Initialize the return button
            $('#' + _buttons.returnButton).click(function () {
                const url = _config.payment.url.sessionEndpoint + _sessionRoutes.setCurrentStep;
                // Set web form session step and index
                /* TO-DO: 
                _apn.SetSessionCurrentStep(url,
                    _session.redirect.sessionId, _session.redirect.stepId,
                    _session.redirect.stepIndex, function (response) {
                        // Redirect to the payment return step
                        window.location.href = _session.redirect.location.href;
                    }); */
            });
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
     * Handles the payment declined step.
     * @returns {void}
     */
    const _handlePaymentDeclinedStep = function () {

        //console.log("_handlePaymentDeclinedStep");
        try {
            // Ensure no error is pending
            if (_error.occured) return;

            // Initialize the cancel button
            $('#' + _buttons.cancelButton).click(function () {
                // Redirect to the referrer web site
                window.location.href = _session.config.originUrl;
            });

            // Initialize the return button
            $('#' + _buttons.returnButton).click(function () {
                const url = _config.payment.url.sessionEndpoint + _sessionRoutes.setCurrentStep;
                // Set web form session step and index
                /* TO-DO: 
                _apn.SetSessionCurrentStep(url,
                    _session.redirect.sessionId, _session.redirect.stepId,
                    _session.redirect.stepIndex, function (response) {
                        // Redirect to the payment return step
                        window.location.href = _session.redirect.location.href;
                    }); */
            });
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
     * Handles the payment exception step.
     * @returns {void}
     */
    const _handlePaymentExceptionStep = function () {

        //console.log("_handlePaymentExceptionStep");
        try {
            // Ensure no error is pending
            if (_error.occured) return;

            // Close session
            _closeSession();

            // Initialize the return home button
            $('#' + _buttons.returnHomeButton).val(_session.config.returnHomeButtonLabel);
            $('#' + _buttons.returnHomeButton).click(function () {
                // Redirect to the referrer web site
                window.location.href = _session.config.originUrl;
            });
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
     * Handles the payment redirect step.
     * @returns {void}
     */
    const _handlePaymentRedirectStep = function () {

        //console.log("_handlePaymentRedirectStep");
        try {
            // Ensure no error is pending
            if (_error.occured) return;

            // Ensure the browser's back button returns to the last step
            window.history.replaceState({}, _session.last.title, _session.last.location.href);

            // Initialize data and redirect
            _app.PaymentRedirect(_config, _session);
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
     * Handles the submission step.
     * @returns {void}
     */
    const _handleSubmissionStep = function () {

        //console.log("_handleSubmissionStep");
        try {
            // Ensure no error is pending
            if (_error.occured) return;

            // Close session
            _closeSession();

            // Initialize the return home button
            $('#' + _buttons.returnHomeButton).val(_session.config.returnHomeButtonLabel);
            $('#' + _buttons.returnHomeButton).click(function () {
                // Redirect to the referrer web site
                window.location.href = _session.config.originUrl;
            });
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
     * Handles regular web form steps during the initialization 
     * of the module. Mainly saves the form data in the session
     * storage and adds events on form change and on before unload.
     * @param {object} formElement the liquid form element.
     * @returns {void}
     */
    const _handleAdvancedFormStep = function (formElement) {

        //console.log("_handleAdvancedFormStep");
        try {
            /**
              * Hides fields that have been marked as hidden
              * in the configuration.
              * @returns {void}
              */
            const hideStepFields = function () {

                /**
                 * Hides a particular step field.
                 * @returns {void}
                 */
                const hideStepField = function (key) {
                    let arr = _config.behaviour.hiddenFields[key];
                    arr.forEach(function (item, index) {
                        $('#' + item).closest('tr').hide();
                    });
                }

                // Ensure no error is pending
                if (_error.occured) return;
                // Proceed only if behaviour and hidden fields exist
                if (_config.behaviour == null || _config.behaviour.hiddenFields == null) return;
                // Loop in the available hidden fields
                for (let key in _config.behaviour.hiddenFields) {
                    switch (key) {
                        case "ALL":
                        case _config.portal.stepId:
                            hideStepField(key);
                            break;
                        case _config.portal.formId:
                            let previousButton = _apd.GetElement(_buttons.previousButton);
                            if (previousButton == null) hideStepField(key);
                            break;
                    }
                }
            };

            /**
              * Loads and executes step actions.
              * "stepId/ALL": {[{elementId, eventName, function}, {...}]}
              * @returns {void}
              */
            const executeStepActions = function () {

                /**
                 * Loads a particular step action.
                 * @returns {void}
                 */
                const loadStepAction = function (key) {
                    let array = _config.behaviour.actions[key]; // External or parent array
                    if (array == null || !Array.isArray(array)) throw new Error("Configuration error in array.");
                    // Loop in the available parent arrays
                    for (let i = 0; i < array.length; i++) {
                        let obj = array[i];
                        if (obj == null || typeof obj !== "object") throw new Error("Configuration error in array object.");
                        if (obj.callBack !== null) {
                            if (obj.elementId == null || obj.eventName == null ||
                                _isNullOrWhiteSpace(obj.elementId) || _isNullOrWhiteSpace(obj.eventName)) {
                                obj.callBack(); // Call the function
                            }
                            else {
                                // 
                                let element = document.getElementById(obj.elementId);
                                if (element !== null) element.addEventListener(obj.eventName, obj.callBack());
                            }
                        }
                    }
                }

                // Ensure no error is pending
                if (_error.occured) return;
                // Proceed only if behaviour and actions exist
                if (_config.behaviour == null || _config.behaviour.actions == null) return;
                // Loop in the available actions
                for (let key in _config.behaviour.actions) {
                    switch (key) {
                        case "ALL":
                        case _config.portal.stepId:
                            loadStepAction(key);
                            break;
                        case _config.portal.formId:
                            let previousButton = _apd.GetElement(_buttons.previousButton);
                            if (previousButton == null) loadStepAction(key);
                            break;
                    }
                }
            };

            /**
             * Sets up the event listeners for the web form step buttons.
             * @param {object} formElement the liquid form element.
             * @returns {void}
             */
            const setupButtonEventListeners = function (formElement) {

                // Get next button element
                let nextButtonElement = _apd.GetElement(_buttons.nextButton);
                if (nextButtonElement != null) {
                    // Only if the submit button behaviour is overridden
                    if (_config.behaviour.isSubmitOverridden === true &&
                        _config.buttons.submitButtonLabel === nextButtonElement.value) {
                        // Clear the OOB event handlers
                        _apd.SetAttribute(formElement, "onsubmit", null);
                        _apd.SetAttribute(nextButtonElement, "onclick", null);
                        // Add click event to the button
                        nextButtonElement.addEventListener("click", function () {
                            //console.log("_submitButtonClick");
                            // Change button label
                            _apd.SetAttribute(nextButtonElement, "value", _buttonProcessingLabel);
                            // Save session data
                            _saveSession();
                            // Redirect to the payment gateway
                            window.location.href = _routes.paymentRedirect + "?sessionid=" + _session.current.sessionId;
                        });
                    }
                    else {
                        // Add click event to the button
                        nextButtonElement.addEventListener("click", function () {
                            //console.log("_nextButtonClick");
                            // Save session data
                            _saveSession();
                        });
                    }
                }

                // Get previous button element
                let previousButtonElement = _apd.GetElement(_buttons.previousButton);
                if (previousButtonElement != null) {
                    // Add click event to the button
                    previousButtonElement.addEventListener("click", function () {
                        //console.log("_previousButtonClick");
                        // Save session data
                        _saveSession();
                    });
                }
            };

            // Ensure no error is pending
            if (_error.occured) return;

            // Save form
            _saveFormData();

            // Hide step fields
            hideStepFields();

            // Execute step actions
            executeStepActions();

            // Setup button event listeners
            setupButtonEventListeners(formElement);
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
      * A method to initialize the module. Must be called first when using this module.
      * @param {Object} config required and must be set in the web template
      * @returns {void}
      */
    const _initialize = function (config, context) {

        //console.log("_initialize");
        try {
            if (config == null) throw new Error(_errors.configNotFound);
            if (jQuery == null) throw new Error(_errors.jQueryIsRequiredError);

            // Initialize config
            _config = config;

            // Initialize session
            _initializeSession();

            // Validate session
            _validateSession();

            // Initialize events
            _initializeEvents();

            // Handle routes business logic
            switch (window.location.pathname.toLowerCase()) {
                case _routes.errorRedirect:
                    _handleErrorRedirectStep();
                    break;

                case _routes.paymentAccepted:
                    _handlePaymentAcceptedStep();
                    break;

                case _routes.paymentCancelled:
                    _handlePaymentCancelledStep();
                    break;

                case _routes.paymentDeclined:
                    _handlePaymentDeclinedStep();
                    break;

                case _routes.paymentException:
                    _handlePaymentExceptionStep();
                    break;

                case _routes.paymentRedirect:
                    _handlePaymentRedirectStep();
                    break;

                default:
                    // Advanced forms steps only contain liquid form element
                    let formElement = _apd.GetElement(_liquidFormElementId);
                    if (formElement == null) {
                        // Consider that step as a submission step
                        _handleSubmissionStep();
                    }
                    else {
                        // Advanced forms steps only
                        $('#aylos_portal_adxwebform').val(_nullify(_config.portal.formId));
                        $('#aylos_portal_adxwebformstep').val(_nullify(_config.portal.stepId));
                        _handleAdvancedFormStep(formElement);
                    }
            }
            //console.log(_messages.webFormsInitialized);
            //console.log("jQuery version: " + jQuery.fn.jquery);
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
      * Initializes the events. 
      * @returns {void}
      */
    const _initializeEvents = function () {

        //console.log("_initializeEvents");
        try {
            /**
              * Initializes the button click event listeners. 
              * @returns {void}
              */
            const initializeButtonClickEventListeners = function () {

                try {
                    let buttons = document.querySelectorAll("input[type=button]");
                    for (let i = 0, len = buttons.length; i < len; i++) {
                        buttons[i].buttonPressed = false;
                        buttons[i].addEventListener("mouseup", function (event) {
                            this.buttonPressed = true;
                        }, true);
                    }
                }
                catch (e) {
                    _handleError(e);
                }
            }

            /**
              * Initialize the window event listeners.
              * @returns {void}
              */
            const initializeWindowEventListeners = function () {

                try {
                    // Initialize on unload event listener
                    window.addEventListener("unload", _onWindowUnload);
                    // Initialize on before unload event listener
                    window.addEventListener("beforeunload", _onWindowBeforeUnload);
                }
                catch (e) {
                    _handleError(e);
                }
            }

            // Ensure no error is pending
            if (_error.occured) return;
            // Handle routes business logic
            switch (window.location.pathname.toLowerCase()) {
                case _routes.errorRedirect:
                case _routes.paymentRedirect:
                case _routes.paymentAccepted:
                case _routes.paymentException:
                    // Nothing to do here
                    break;

                case _routes.paymentCancelled:
                case _routes.paymentDeclined:
                    // Initialize the window event listeners
                    initializeWindowEventListeners();

                    // Initialize button click event listener
                    initializeButtonClickEventListeners();
                    break;

                default:
                    // Advanced forms steps only contain liquid form element
                    let formElement = _apd.GetElement(_liquidFormElementId);
                    if (formElement != null) {
                        // Initialize on form change event listener
                        formElement.addEventListener("change", _onFormChange);
                    }

                    // Initialize button click event listener
                    initializeButtonClickEventListeners();

                    // Initialize the window event listeners
                    initializeWindowEventListeners();
            }
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
      * Initializes the in memory session. 
      * @returns {void}
      */
    const _initializeSession = function () {

        //console.log("_initializeSession");
        try {
            // Ensure no error is pending
            if (_error.occured) return;
            // Initialize current session
            _session.current.entityId = _nullify(_config.portal.entityId);
            _session.current.entityName = _nullify(_config.portal.entityName);
            _session.current.formId = _nullify(_config.portal.formId);
            _session.current.pageId = _nullify(_config.portal.pageId);
            _session.current.sessionId = _nullify(_config.portal.sessionId);
            _session.current.stepId = _nullify(_config.portal.stepId);
            _session.current.stepIndex = _getReturnStep(_session.current.stepId);

            // Load the session from the storage
            let session = _aps.RetrieveItem(_sessionKey);
            if (!_isNullOrWhiteSpace(session)) {
                // Parse session
                let s = JSON.parse(session);
                _session.config = s.config;
                _session.last = s.last;
                _session.redirect = s.redirect;
                _session.payment = s.payment;
                // Keep web form session alive
                // TO-DO: _apn.KeepSessionAlive(_session.current.sessionId, JSON.stringify(_session));
            }
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
      * Clears the in memory form and session data 
      * and closes the web session. 
      * @returns {void}
      */
    const _closeSession = function () {

        //console.log("_closeSession");
        try {
            // Ensure no error is pending
            if (_error.occured) return;

            // Check if debugging is enabled
            if (_config.behaviour.debugging === true) {
                //console.log(_errors.debuggingIsEnabled);
            }
            else {
                // Remove all storage items
                if (!_isNullOrWhiteSpace(_aps.RetrieveItem(_dataKey))) _aps.RemoveItem(_dataKey);
                if (!_isNullOrWhiteSpace(_aps.RetrieveItem(_sessionKey))) _aps.RemoveItem(_sessionKey);

                // Invalidate the session
                if (_session != null && _session.last != null && !_isNullOrWhiteSpace(_session.last.sessionId)) {
                    const url = _config.payment.url.sessionEndpoint + _sessionRoutes.invalidate;
                    // TO-DO: _apn.InvalidateSession(url, _session.last.sessionId);
                }
            }
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
     * An on form change event handler. We expect that by adding the 
     * event handler on the form we are capturing all form children 
     * fields' events. 
     * @param {Event} event the event triggered by a field value change.
     * @returns {void}
     */
    const _onFormChange = function (event) {

        //console.log("_onFormChange");
        try {
            // Make sure that you get the event
            event = event || window.event;

            // Ensure no error is pending
            if (_error.occured) {
                if (event.cancelable === true) event.preventDefault();
                return;
            }

            // Save form
            _saveFormData();
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
      * An on window before unload event handler. 
      * @param {Event} event the event triggered before window unload.
      * @returns {void}
      */
    const _onWindowBeforeUnload = function (event) {

        //console.log("_onWindowBeforeUnload");
        try {
            // Make sure that you get the event
            event = event || window.event;

            // Ensure no error is pending
            if (_error.occured) {
                if (event.cancelable === true) event.preventDefault();
                return;
            }

            // Handle routes business logic
            switch (window.location.pathname.toLowerCase()) {
                case _routes.errorRedirect:
                case _routes.paymentRedirect:
                case _routes.paymentAccepted:
                case _routes.paymentException:
                    // Nothing to do here;
                    break;

                case _routes.paymentCancelled:
                case _routes.paymentDeclined:
                    // If button not pressed popup message
                    if (_apd.IsButtonPressed() === false) {
                        if (event != null) event.returnValue = _messages.confirmSessionClosing;
                        return _messages.confirmSessionClosing;
                    }
                    break;

                default:
                    // Advanced forms steps only contain liquid form element
                    let formElement = _apd.GetElement(_liquidFormElementId);
                    if (formElement != null) {
                        // If button not pressed popup message
                        if (_apd.IsButtonPressed() === false) {
                            if (event != null) event.returnValue = _messages.confirmSessionClosing;
                            return _messages.confirmSessionClosing;
                        }
                    }
            }
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
      * An on window unload event handler. 
      * @param {Event} event the event triggered before window unload.
      * @returns {void}
      */
    const _onWindowUnload = function (event) {

        //console.log("_onWindowUnload");
        try {
            // Make sure that you get the event
            event = event || window.event;

            // Ensure no error is pending
            if (_error.occured) {
                if (event.cancelable === true) event.preventDefault();
                return;
            }

            // Handle routes business logic
            switch (window.location.pathname.toLowerCase()) {
                case _routes.errorRedirect:
                case _routes.paymentRedirect:
                case _routes.paymentAccepted:
                case _routes.paymentDeclined:
                    // Nothing to do here;
                    break;

                case _routes.paymentException:
                case _routes.paymentCancelled:
                    // If button not pressed close session
                    if (_apd.IsButtonPressed() === false) _closeSession();
                    break;

                default:
                    // Advanced forms steps only contain liquid form element
                    let formElement = _apd.GetElement(_liquidFormElementId);
                    if (formElement != null) {
                        // If button not pressed close session
                        if (_apd.IsButtonPressed() === false) _closeSession();
                    }
            }
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
     * A function to persist the form data in session storage.
     * @returns {void}
     */
    const _saveFormData = function () {

        //console.log("_saveFormData");
        try {
            // Ensure no error is pending
            if (_error.occured) return;

            // Advanced forms steps only contain liquid form element
            let formElement = _apd.GetElement(_liquidFormElementId);
            if (formElement != null) {
                // Extract the form data
                let jsonObj = _formToJSON(formElement.elements);

                // Retrieve saved form data
                let storageObj = _aps.RetrieveItem(_dataKey) ? JSON.parse(_aps.RetrieveItem(_dataKey)) : {};

                // Update the storage data
                for (let key in jsonObj) { storageObj[key] = jsonObj[key]; }

                // Save the portal data in the storage
                _aps.SaveItem(_dataKey, JSON.stringify(storageObj, null, 3));

                // Update the local data
                for (let key in storageObj) { _data[key] = storageObj[key]; }
            }
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
     * A function to persist the session data in session storage.
     * @returns {void}
     */
    const _saveSession = function () {

        //console.log("_saveSession");
        try {
            // Ensure no error is pending
            if (_error.occured) return;

            // Load the session from the storage
            let session = _aps.RetrieveItem(_sessionKey);
            if (_isNullOrWhiteSpace(session)) {
                // Update last session
                _session.last = _session.current;

                // Empty current and redirect session
                _session.current = {};
                _session.redirect = {};

                // Update config session items
                _session.config.originUrl = _isNullOrWhiteSpace(_config.behaviour.originUrl) ? _defaultOriginUrl : _config.behaviour.originUrl;
                _session.config.returnHomeButtonLabel = _isNullOrWhiteSpace(_config.buttons.returnHomeButtonLabel) ? _defaultReturnHomeButtonLabel : _config.buttons.returnHomeButtonLabel;
                _session.config.serviceLabel = _nullify(_config.behaviour.serviceLabel);
                _session.config.slaLabel = _nullify(_config.behaviour.slaLabel);

                // Save session in the storage
                _aps.SaveItem(_sessionKey, JSON.stringify(_session, null, 3));
            }
            else {
                let s = JSON.parse(session);

                // Update last session
                s.last = _session.current;

                // Update redirect session if current is a payment return step
                if (_getReturnStep(_session.current.stepId)) s.redirect = _session.current;

                // Clear current session
                s.current = {};

                // Update config session
                s.config = _session.config;

                // Update payment session
                s.payment = _session.payment;

                // Save session in the storage
                _aps.SaveItem(_sessionKey, JSON.stringify(s, null, 3));
            }
        }
        catch (e) {
            _handleError(e);
        }
    };

    /**
      * Validates the session. 
      * @returns {void}
      */
    const _validateSession = function () {

        //console.log("_validateSession");
        try {
            // Ensure no error is pending
            if (_error.occured) return;

            // Load the session from the storage
            let session = _aps.RetrieveItem(_sessionKey);

            // Handle routes session validation logic
            switch (window.location.pathname.toLowerCase()) {
                case _routes.errorRedirect: // error page
                    // Nothing to do here
                    break;

                case _routes.paymentAccepted: // payment related pages
                case _routes.paymentException:
                case _routes.paymentCancelled:
                case _routes.paymentDeclined:
                case _routes.paymentRedirect:
                    if (_session.current.pageId == null) throw new Error(_errors.pageCannotBeIdentified);
                    if (_isNullOrWhiteSpace(session)) throw new Error(_errors.sessionIsCorrupted);
                    let s = JSON.parse(session);
                    if (s == null ||
                        s.last == null || s.last.formId == null || s.last.pageId == null || s.last.sessionId == null ||
                        s.redirect == null || s.redirect.formId == null || s.redirect.pageId == null || s.redirect.pageId == null
                    ) throw new Error(_errors.sessionIsCorrupted);
                    if (_isNullOrWhiteSpace(_session.current.sessionId) ||
                        _session.current.sessionId !== s.last.sessionId ||
                        _session.current.sessionId !== s.redirect.sessionId) throw new Error(_errors.sessionCannotBeIdentified);
                    break;

                default:
                    // Advanced forms steps only contain liquid form element
                    let formElement = _apd.GetElement(_liquidFormElementId);
                    if (formElement != null) {
                        // Advanced form steps only
                        if (_isNullOrWhiteSpace(_session.current.formId)) throw new Error(_errors.formCannotBeIdentified);
                        if (_isNullOrWhiteSpace(_session.current.pageId)) throw new Error(_errors.pageCannotBeIdentified);
                        if (_isNullOrWhiteSpace(session)) {
                            if (!_isNullOrWhiteSpace(_session.current.sessionId)) throw new Error(_errors.sessionCannotBeIdentified);
                        }
                        else {
                            let s = JSON.parse(session);
                            if (s == null || s.last == null || s.last.formId == null || s.last.pageId == null) throw new Error(_errors.sessionIsCorrupted);
                            if (_session.current.formId !== s.last.formId) throw new Error(_errors.formCannotBeIdentified);
                            if (_session.current.pageId !== s.last.pageId) throw new Error(_errors.pageCannotBeIdentified);
                            if (!_isNullOrWhiteSpace(s.last.sessionId) && _session.current.sessionId !== s.last.sessionId) throw new Error(_errors.sessionCannotBeIdentified);
                        }
                    }
            }
        }
        catch (e) {
            _handleError(e);
        }
    };

    // Module references
    const _ap = Aylos.Portal; // TO-DO:
    const _apd = Aylos.Portal.DomHelpers;
    const _apn = Aylos.Portal.Network; // TO-DO:
    const _app = Aylos.Portal.Payments;
    const _aps = Aylos.Portal.Storage;

    // Constants
    const _defaultOriginUrl = "https://aylos.powerappsportals.com/";
    const _defaultReturnHomeButtonLabel = "Return to the home page";
    const _entityIdFieldName = "EntityFormView_EntityID";
    const _entityNameFieldName = "EntityFormView_EntityName";
    const _excludedFieldIds = "__EVENTTARGET,__EVENTARGUMENT,__EVENTVALIDATION,__VIEWSTATE,__VIEWSTATEGENERATOR,__VIEWSTATEENCRYPTED,confirmOnExit,confirmOnExitMessage";
    const _liquidFormElementId = "liquid_form";

    // Buttons
    const _buttonProcessingLabel = "Processing...";
    const _buttons = {
        cancelButton: "CancelButton",
        nextButton: "NextButton",
        previousButton: "PreviousButton",
        restartButton: "RestartButton",
        returnButton: "ReturnButton",
        returnHomeButton: "ReturnHomeButton"
    };

    // Config - Must be initialized in the web template.
    var _config = {
        behaviour: {
            actions: {
                // in the first step use the form id (guid) | for all steps use the word 'ALL'
                "ALL": [ // Step: ALL
                    {
                        // Add event listener on DOM content loaded
                        elementId: null,
                        eventName: null,
                        callBack: document.addEventListener("DOMContentLoaded", function () { })
                    },
                ],
            },
            debugging: false, //set true only while debugging!!!
            isSubmitOverridden: true, //if true the submit button behavior will be overriden
            hiddenFields: {
                // in the first step use the form id (guid) | for all steps use the word 'ALL'
                "the step id": ["the element id", "the element id",]
            },
            originUrl: "https://aylos.powerappsportals.com/", //expected parameter from the caller page
            serviceLabel: null, //add the service label
            serviceReferenceNumberMapping: null, //add the mapping to the service reference number field in the form 'entityname#attributename'
            primaryEntityId: null, // entity_name#EntityFormView_EntityID
            slaLabel: null //add the text that will be displayed on successful and uncertain payments
        },
        buttons: {
            returnHomeButtonLabel: "Return to the home page",
            submitButtonLabel: "Pay" //if value will apply business logic
        },
        payment: {
            fieldMapping: {
                //add the mappings to the payment fields in the form 'entityname#attributename'
                orderId: null,
                amount: null,
                firstName: null,
                lastName: null,
                customerName: null,
                email: null,
                postCode: null,
                address: null,
                county: null,
                town: null,
                telephone: null
            },
            returnStep: {
                //add the primary key and index of the step to return from cancelled or declined payments
                id: null,
                index: null
            },
            url: {
                //add the endpoints for SHA calculation, sessions, and the payment gateway landing page URL
                calculateShaApi: null,
                paymentGateway: null,
                sessionEndpoint: null
            }
        },
        portal: {
            entityId: "{{ request.params['entityid'] }}", //available on page redirects
            entityName: "{{ request.params['entityname'] }}", //available on page redirects
            formId: "{{ page.adx_webform.id }}", //available through web forms steps only
            pageId: "{{ page.id }}", //available to all portal pages
            sessionId: "{{ request.params['sessionid'] }}", //available through web forms steps only
            stepId: "{{ request.params['stepid'] }}" //available through web forms steps only
        }
    }

    // Errors
    const _error = {
        occured: false,
        error: null,
        message: null
    };
    const _errors = {
        configNotFound: "Configuration object is required.",
        debuggingIsEnabled: "Oops, debugging is enabled. Please ensure that you disable debugging before deployment.",
        formCannotBeIdentified: "The form cannot be identified.",
        jQueryIsRequiredError: "Oops, although jQuery was expected it not there. Cannot continue execution.",
        pageCannotBeIdentified: "The page cannot be identified.",
        sessionIsCorrupted: "The session is corrupted.",
        sessionCannotBeIdentified: "The session cannot be identified."
    };

    // Messages
    const _messages = {
        confirmSessionClosing: // the custom confirmation message is not supported by the modern browsers
            "Warning!!! You are about to navigate away but your application has not been " +
            "completed as yet. If you wish to abandon your application press 'OK', " +
            "otherwise press 'Cancel' to continue with your application.",
        webFormsInitialized: "Advanced forms library initialized successfully."
    };

    // Routes
    const _routes = {
        errorRedirect: "/error/",
        paymentAccepted: "/payment-accepted/",
        paymentDeclined: "/payment-declined/",
        paymentCancelled: "/payment-cancelled/",
        paymentException: "/payment-uncertain/",
        paymentRedirect: "/payment-redirect/"
    }

    // Session Routes
    const _sessionRoutes = {
        invalidate: "/invalidatesession/",
        keepAlive: "/continuesession/",
        setCurrentStep: "/setsessioncurrentstep/"
    }

    // Session items
    const _dataKey = "data";
    const _data = {};
    const _sessionKey = "session";
    const _session = {
        config: {
            originUrl: null,
            returnHomeButtonLabel: null,
            serviceLabel: null,
            serviceReferenceNumber: null,
            slaLabel: null
        },
        current: {
            contactid: null,
            entityId: null,
            entityName: null,
            formId: null,
            pageId: null,
            sessionId: null,
            extendedSessionId: null,
            stepId: null,
            stepIndex: null,
            location: window.location,
            title: document.title
        },
        last: {
            contactid: null,
            entityId: null,
            entityName: null,
            formId: null,
            pageId: null,
            sessionId: null,
            extendedSessionId: null,
            stepId: null,
            stepIndex: null,
            location: null,
            title: null
        },
        redirect: {
            contactid: null,
            entityId: null,
            entityName: null,
            formId: null,
            pageId: null,
            sessionId: null,
            extendedSessionId: null,
            stepId: null,
            stepIndex: null,
            location: null,
            title: null
        },
        payment: {
            details: {
                orderId: null,
                amount: null,
                firstName: null,
                lastName: null,
                customerName: null,
                email: null,
                postCode: null,
                address: null,
                county: null,
                town: null,
                telephone: null
            },
            url: {
                acceptUrl: window.location.origin + _routes.paymentAccepted,
                cancelUrl: window.location.origin + _routes.paymentCancelled,
                declineUrl: window.location.origin + _routes.paymentDeclined,
                exceptionUrl: window.location.origin + _routes.paymentException
            },
        }
    };

    /**
     * Public methods
     */
    this.Data = _data;
    this.Initialize = _initialize;
    this.IsNullOrWhiteSpace = _isNullOrWhiteSpace;
    this.Session = _session;

}).apply(Aylos.Portal.AdvancedForms);

(function () {
    /**
     * Aylos.Portal.DomHelpers
     */

    "use strict";

    /**
     * A function to create a new DOM input element.
     * @param {!string} parentId the id of the parent element.
     * @param {!string} elementId the id.
     * @param {!string} elementType the type.
     * @param {string} elementValue the value.
     */
    const _createInputElement = function (parentId, elementId, elementType, elementValue) {

        if (parentId == null) throw new Error("The parentId argument is null or undefined.");
        if (typeof parentId !== "string") throw new Error("The parentId argument type must be string.");

        if (elementId == null) throw new Error("The elementId argument is null or undefined.");
        if (typeof elementId !== "string") throw new Error("The elementId argument type must be string.");

        if (elementType == null) throw new Error("The elementType argument is null or undefined.");
        if (typeof elementType !== "string") throw new Error("The elementType argument type must be string.");

        let parentElement = _getElement(parentId);
        if (parentElement == null) throw new Error("DOM element '" + parentId + "' does not exist.");
        if (_getElement(elementId)) throw new Error("DOM element '" + elementId + "' already exists.");

        let element = document.createElement("input");
        element.id = elementId;
        element.type = elementType;
        element.value = elementValue;
        parentElement.appendChild(element);
    };

    /**
     * Checks if an input is a button, because buttons do not contribute any values.
     * @param  {!Element} element the element to check.
     * @returns {Boolean} true if the element is a button, false if not.
     */
    const _isButton = function (element) {

        if (element == null) throw new Error("The element argument is null or undefined.");
        if (typeof element !== "object") throw new Error("The element argument type must be object.");

        return element.type === "button";
    };

    /**
      * Checks if a button has been clicked. Otherwise
      * notifies the user if they wish to navigate away
      * from the application forms. 
      * @returns {void}
      */
    const _isButtonPressed = function () {

        try {
            // Ensure no error is pending
            if (_error.occured) return;
            // Check if a button pressed
            let buttons = document.querySelectorAll("input[type=button]");
            for (let i = 0, len = buttons.length; i < len; i++) {
                if (buttons[i].buttonPressed === true) return true;
            }
            return false;
        }
        catch (e) {
            _handleError(e);
        }
    }

    /**
     * Checks if an input is a checkbox, because checkboxes allow multiple values.
     * @param  {Element} element  the element to check
     * @returns {Boolean} true if the element is a checkbox, false if not
     */
    const _isCheckbox = function (element) {

        if (element == null) throw new Error("The element argument is null or undefined.");
        if (typeof element !== "object") throw new Error("The element argument type must be object.");

        return element.type === "checkbox";
    };

    /**
     * Checks if an input is a `select` with the `multiple` attribute.
     * @param  {Element} element  the element to check
     * @returns {Boolean} true if the element is a multiselect, false if not
     */
    const _isMultiSelect = function (element) {

        if (element == null) throw new Error("The element argument is null or undefined.");
        if (typeof element !== "object") throw new Error("The element argument type must be object.");

        return element.options && element.multiple;
    };

    /**
     * Checks that an element has a non-empty name property.
     * @param  {Element} element  the element to check
     * @returns {Boolean} true if the element is an input, false if not
     */
    const _isValidElement = function (element) {

        if (element == null) throw new Error("The element argument is null or undefined.");
        if (typeof element !== "object") throw new Error("The element argument type must be object.");

        return element.name;
    };

    /**
     * Checks if an element’s value can be saved (e.g. not an unselected checkbox).
     * @param  {Element} element  the element to check
     * @returns {Boolean} true if the value should be added, false if not
     */
    const _isValidValue = function (element) {

        if (element == null) throw new Error("The element argument is null or undefined.");
        if (typeof element !== "object") throw new Error("The element argument type must be object.");

        return (!['checkbox', 'radio'].includes(element.type) || element.checked);
    };

    /**
      * Gets a DOM element by id.
      * @param {string} id the id of the DOM element.
      * @returns {DOM Element} the DOM element.
      */
    const _getElement = function (id) {

        return id == null ? null : document.getElementById(id);
    };

    /**
     * Gets the object for the specified value.
     * @param {Object} obj the object.
     * @param {string} val the value of the object.
     * @returns {void}
     */
    const _getObjectByValue = function (obj, val) {

        if (obj == null) throw new Error("The obj argument is null or undefined.");
        if (typeof obj !== "object") throw new Error("The obj argument type must be object.");

        if (val == null) throw new Error("The val argument is null or undefined.");
        if (typeof val !== "string") throw new Error("The val argument type must be string.");

        for (let key in obj) {
            if (obj[key] === val) return [key, obj[key]];
        }
        return null;
    }

    /**
     * Gets the parameters extracted from the URL.
     * @returns {Object}
     */
    const _getUrlParams = function () {

        const match,
            pl = /\+/g,  // Regex for replacing addition symbol with a space
            search = /([^&=]+)=?([^&]*)/g,
            decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
            query = window.location.search.substring(1);

        let params = {};
        while (match = search.exec(query)) {
            params[decode(match[1])] = decode(match[2]);
        }

        return params;
    }

    /**
     * Retrieves the selected options from a multi-select as an array.
     * @param  {HTMLOptionsCollection} options  the options for the select
     * @returns {Array} an array of selected option values
     */
    const _getSelectValues = function (options) {
        return [].reduce.call(options, function (values, option) {
            return option.selected ? values.concat(option.value) : values;
        }, []);
    };

    /**
     * Sets the value of the specified attribute for the given element. 
     * @param {!object} documentElement the DOM element.
     * @param {!string} attributeName the attribute name.
     * @param {string} attributeValue the attribute value.
     * @returns {void}
     */
    const _setAttribute = function (documentElement, attributeName, attributeValue) {

        if (documentElement == null) throw new Error("The documentElement argument is null or undefined.");
        if (typeof documentElement !== "object") throw new Error("The documentElement argument type must be object.");

        if (attributeName == null) throw new Error("The attributeName argument is null or undefined.");
        if (typeof attributeName !== "string") throw new Error("The attributeName argument type must be string.");

        let attr = documentElement.getAttribute(attributeName);
        if (attributeValue == null) {
            if (attr != null) documentElement.removeAttribute(attributeName);
        } else {
            documentElement.setAttribute(attributeName, attributeValue);
        }
    };

    /**
     * Public members
     */
    this.CreateInputElement = _createInputElement;
    this.IsButton = _isButton;
    this.IsButtonPressed = _isButtonPressed;
    this.IsCheckbox = _isCheckbox;
    this.IsMultiSelect = _isMultiSelect;
    this.IsValidElement = _isValidElement;
    this.IsValidValue = _isValidValue;
    this.GetElement = _getElement;
    this.GetObjectByValue = _getObjectByValue;
    this.GetUrlParams = _getUrlParams;
    this.GetSelectValues = _getSelectValues;
    this.SetAttribute = _setAttribute;

}).apply(Aylos.Portal.DomHelpers);

(function () {
    /**
     * Aylos.Portal.Network
     */

    "use strict";

    // TO-DO: REMOVE THIS https://www.dancingwithcrm.com/powerapps-portal-web-api-deep-dive/

    /**
     * A function to call asyncronously the portal web api.
     * @param {!Object} ajaxOptions the HTTP request parameters and the payload that will be submitted.
     * @param {!boolean} requiresLogin indicates if the request requires the user to be logged in.
     * @returns {Object} JSON object.
     */
    const _callPortalAPI = function (ajaxOptions, requiresLogin) {

        if (ajaxOptions == null) throw new Error("The ajaxOptions argument is undefined.");
        if (typeof ajaxOptions !== "object") throw new Error("The ajaxOptions argument type must be object.");

        if (requiresLogin == null) throw new Error("The requiresLogin argument is undefined.");
        if (typeof requiresLogin !== "boolean") throw new Error("The requiresLogin argument type must be boolean.");

        let deferredAjax = $.Deferred();

        shell.getTokenDeferred()
            .done(function (token) {
                debugger;
                if (!ajaxOptions.headers) {
                    $.extend(ajaxOptions, {
                        headers: {
                            "__RequestVerificationToken": token
                        }
                    });
                } else {
                    ajaxOptions.headers["__RequestVerificationToken"] = token;
                }

                $.ajax(ajaxOptions)
                    .done(function (data, textStatus, jqXHR) {
                        // TO-DO: check the below 
                        debugger;
                        if (requiresLogin) validateLoginSession(data, textStatus, jqXHR, deferredAjax.resolve);
                    }).fail(function () {
                        // TO-DO: check the below 
                        debugger;
                        deferredAjax.reject; // AJAX
                    });

        }).fail(function () {
            debugger;
            deferredAjax.rejectWith(this, arguments); // on token failure pass the token AJAX and args
        });

        return deferredAjax.promise();
    };

    /**
     * A function to call asyncronously a service using HTTP POST.
     * Default Request Header: "Content-type", "application/json; charset=utf-8"
     * @param {!string} data the payload that will be submitted to the service.
     * @param {!string} url the URL of the service endpoint.
     * @param {?Object} callback the function to call when the call completes successfully.
     * @returns {void} .
     */
    const _callService = function (data, url, callback) {

        if (data == null) throw new Error("The data argument is undefined.");
        if (typeof data !== "string") throw new Error("The data argument type must be string.");

        if (url == null) throw new Error("The url argument is undefined.");
        if (typeof url !== "string") throw new Error("The url argument type must be string.");

        let xhr = new XMLHttpRequest();
        xhr.onerror = function (e) {
            window.console.error(xhr.statusText);
            throw new Error(_errors.XhrError);
        };
        xhr.ontimeout = function (e) {
            window.console.error(xhr.statusText);
            throw new Error(_errors.XhrTimeout);
        };
        xhr.onreadystatechange = function () {
            if (xhr.readyState == XMLHttpRequest.DONE && xhr.status == 200) {
                if (typeof (callback) === "function") callback(xhr.responseText);
            }
        }
        xhr.open("POST", url, true);
        xhr.setRequestHeader("Content-type", "application/json; charset=utf-8");
        xhr.send(data);
    };

    /*
     * Invalidate session.
     * @param {!string} extendedSessionId the extended session primary key (GUID).
     * @param {?Function} callback reference following successful completion.
     * @returns {void} .
     */
    const _invalidateSession = function (extendedSessionId, callback) {

        if (extendedSessionId == null) throw new Error("The extendedSessionId argument is null or undefined.");
        if (typeof extendedSessionId !== "string") throw new Error("The extendedSessionId argument type must be string.");

        _callPortalAPI({
            type: "DELETE",
            url: "/_api/aylos_extendedsessions(" + extendedSessionId + ")",
            contentType: "application/json",
            success: function (res) {
                debugger;
                console.log(res);
                if (typeof (callback) === "function") callback(res);
            },
            error: function (xhr, status, error) {
                debugger;
                console.log(error);
                // TO-DO:
            }
        }, false);
    };

    /*
     * Retrieve session.
     * @param {!string} extendedSessionId the extended session primary key (GUID).
     * @param {?Function} callback reference following successful completion.
     * @returns {void} .
     */
    const _getSessionById = function (extendedSessionId, callback) {

        if (extendedSessionId == null) throw new Error("The extendedSessionId argument is null or undefined.");
        if (typeof extendedSessionId !== "string") throw new Error("The extendedSessionId argument type must be string.");

        _callPortalAPI({
            type: "GET",
            url: "/_api/aylos_extendedsessions(" + extendedSessionId + ")/aylos_extendedsession?$select=aylos_clientcode,aylos_contactid,aylos_session,aylos_webformid,aylos_webformsessionid",
            contentType: "application/json",
            success: function (res) {
                debugger;
                console.log(res);
                if (typeof (callback) === "function") callback(res);
            },
            error: function (xhr, status, error) {
                debugger;
                console.log(error);
                // TO-DO:
            }
        }, false);
    };

    /*
     * Retrieve session.
     * @param {!string} clientCode the extended session primary key (GUID).
     * @param {!string} contactId the extended session primary key (GUID).
     * @param {!string} webformId the extended session primary key (GUID).
     * @param {?Function} callback reference following successful completion.
     * @returns {void} .
     */
    const _getSessionByCompositeKey = function (clientCode, contactId, webformId, callback) {

        if (clientCode == null) throw new Error("The clientCode argument is null or undefined.");
        if (typeof clientCode !== "string") throw new Error("The clientCode argument type must be string.");

        if (contactId == null) throw new Error("The contactId argument is null or undefined.");
        if (typeof contactId !== "string") throw new Error("The contactId argument type must be string.");

        if (webformId == null) throw new Error("The webformId argument is null or undefined.");
        if (typeof webformId !== "string") throw new Error("The webformId argument type must be string.");

        _callPortalAPI({
            type: "GET",
            url: "/_api/aylos_extendedsessions(" + extendedSessionId + ")?$select=aylos_clientcode,aylos_contactid,aylos_session,aylos_webformid,aylos_webformsessionid",
            contentType: "application/json",
            success: function (res) {
                debugger;
                console.log(res);
                if (typeof (callback) === "function") callback(res);
            },
            error: function (xhr, status, error) {
                debugger;
                console.log(error);
                // TO-DO:
            }
        }, false);
    };

    /*
     * Keep session alive.
     * @param {!string} extendedSessionId the extended session primary key (GUID).
     * @param {!string} session the session content.
     * @param {?Function} callback reference following successful completion.
     * @returns {void} .
     */
    const _keepSessionAlive = function (extendedSessionId, session, callback) {

        if (extendedSessionId == null) throw new Error("The extendedSessionId argument is null or undefined.");
        if (typeof extendedSessionId !== "string") throw new Error("The extendedSessionId argument type must be string.");

        if (session == null) throw new Error("The session argument is null or undefined.");
        if (typeof session !== "string") throw new Error("The session argument type must be string.");

        webapi.safeAjax({
            type: "PATCH",
            url: "/_api/aylos_extendedsessions(" + extendedSessionId + ")",
            contentType: "application/json",
            data: JSON.stringify({
                "aylos_session": session
            }),
            success: function (res) {
                debugger;
                console.log(res);
                if (typeof (callback) === "function") callback(res);
            },
            error: function (xhr, status, error) {
                debugger;
                console.log(error);
                // TO-DO:
            }
        }, false);
    };

    // Error messages
    const _errors = {
        XhrError: "The server responded with an error.",
        XhrTimeout: "The server connection has been timed out."
    };

    /**
     * Public members
     */
    this.CallService = _callService;
    this.InvalidateSession = _invalidateSession;
    this.KeepSessionAlive = _keepSessionAlive;

}).apply(Aylos.Portal.Network);

(function () {
    /**
     * Aylos.Portal.Payments
     */

    "use strict";

    /** 
     * A function to get fields included that will be used 
     * for the calculation of the SHA signature. Requires 
     * the payment form to be rendered.
     * @returns {string}
     */
    const _getShaFields = function () {

        let shaFields = {};
        let nonEmptyShaFields = $(".sha").filter("input[value!='']").toArray();
        nonEmptyShaFields.forEach(function (item, index) {
            shaFields[item.name.toUpperCase()] = item.value;
        });
        return JSON.stringify(shaFields);
    };

    /**
      * Initializes the Barclays payment gateway form data.
      * Requires the Barclays payment form to be rendered.
      * @param {Object} config the config object that contains all the session values.
      * @param {Object} session the session object that contains all the session values.
      * @returns {void}
      */
    const _initializeBarclaysPaymentForm = function (config, session) {

        if (config == null) throw new Error("The config argument is null or undefined.");
        if (typeof config !== "object") throw new Error("The config argument type must be string.");

        if (session == null) throw new Error("The session argument is null or undefined.");
        if (typeof session !== "object") throw new Error("The session argument type must be string.");

        // Initialize mandatory form fields
        $("input[name=ORDERID]").val(session.payment.details.orderId);
        $("input[name=AMOUNT]").val(100 * parseFloat(Number(session.payment.details.amount.replace(/[^0-9.-]+/g, ""))).toFixed(2));
        $("input[name=CN]").val(session.payment.details.customerName == null
            ? session.payment.details.firstName + " " + session.payment.details.lastName
            : session.payment.details.customerName);
        $("input[name=EMAIL]").val(session.payment.details.email);
        // Initialize optional form fields
        $("input[name=OWNERZIP]").val(session.payment.details.postCode);
        $("input[name=OWNERADDRESS]").val(session.payment.details.address);
        $("input[name=OWNERCTY]").val(session.payment.details.county);
        $("input[name=OWNERTOWN]").val(session.payment.details.town);
        $("input[name=OWNERTELNO]").val(session.payment.details.telephone);
        // Initialize feedback URLs
        $("input[name=ACCEPTURL]").val(session.payment.url.acceptUrl);
        $("input[name=CANCELURL]").val(session.payment.url.cancelUrl);
        $("input[name=DECLINEURL]").val(session.payment.url.declineUrl);
        $("input[name=EXCEPTIONURL]").val(session.payment.url.exceptionUrl);
        // Initialize feedback query string
        $("input[name=PARAMPLUS]").val(
            "redirectUrl=" + session.redirect.location.origin + session.redirect.location.pathname +
            "&stepid=" + session.redirect.stepId + "&sessionid=" + session.redirect.sessionId +
            "&entityid=" + session.redirect.entityId + "&entityname=" + session.redirect.entityName +
            "&reference=" + session.config.serviceReferenceNumber + "&service=" + session.config.serviceLabel +
            "&sla=" + session.config.slaLabel
            );
        // Initialize the action of the form element
        $("form[name=bcard]").attr("action", config.payment.url.paymentGateway);
        // Initialize the fields required for the SHA signature calculation
        let data = _getShaFields();
        // Calculate the SHA singature and submit form
        _apn.CallService(data, config.payment.url.calculateShaApi, function (response) {
            let sha = JSON.parse(response);
            // Initialize SHA signature form field
            $("input[name=SHASIGN]").val(sha);
            // Submit the payment form - redirects to the payment gateway
            setTimeout(function () { $("form[name=bcard]").submit(); }, 1000);
        });
    };

    // Module references
    const _apn = Aylos.Portal.Network;

    /**
     * Public members
     */
    this.PaymentRedirect = _initializeBarclaysPaymentForm;

}).apply(Aylos.Portal.Payments);

(function () {
    /**
     * Aylos.Portal.Storage
     */

    "use strict";

    /**
    * A function to check availability of local or session storage on the browser.
    * @param {?string} storageType expected values localStorage and sessionStorage.
    * @returns {!boolean}
    */
    const _isAvailable = function (storageType) {

        if (storageType == null) storageType = _storageType.defaultStorageType;
        if (typeof storageType !== "string") throw new Error("The storageType argument type must be string.");
        if (storageType !== _storageType.localStorage && storageType !== _storageType.sessionStorage) throw new Error("The given storageType argument value is not expected.");

        try {
            let storage = window[storageType], x = '__storage_test__';
            storage.setItem(x, x);
            storage.removeItem(x);
            return true;
        }
        catch (e) {
            return e instanceof DOMException && (
                // everything except Firefox
                e.code === 22 ||
                // Firefox
                e.code === 1014 ||
                // test name field too, because code might not be present
                // everything except Firefox
                e.name === 'QuotaExceededError' ||
                // Firefox
                e.name === 'NS_ERROR_DOM_QUOTA_REACHED') &&
                // acknowledge QuotaExceededError only if there's something already stored
                (storage && storage.length !== 0);
        }
    };

    /**
    * A function to remove data from the session storage.
    * @param {!string} key the key of the key/value pair.
    * @param {?string} storageType expected values localStorage and sessionStorage.
    * @returns {void}
    */
    const _removeItem = function (key) {

        if (key == null) throw new Error("The key argument is null or undefined.");
        if (typeof key !== "string") throw new Error("The key argument type must be string.");

        if (!_isAvailable(storageType)) throw new Error(_errors.storageException);

        let storage = window[storageType ?? _storageType.defaultStorageType];

        storage.removeItem(key);
    };

    /**
    * A function to save the data in session storage.
    * @param {!string} key the key of the key/value pair.
    * @param {?string} storageType expected values localStorage and sessionStorage.
    * @returns {string} the value in the form of a string. 
    */
    const _retrieveItem = function (key) {

        if (key == null) throw new Error("The key argument is null or undefined.");
        if (typeof key !== "string") throw new Error("The key argument type must be string.");

        if (!_isAvailable(storageType)) throw new Error(_errors.storageException);

        let storage = window[storageType ?? _storageType.defaultStorageType];

        return storage.getItem(key);
    };

    /**
    * A function to save the data in session storage.
    * @param {!string} key the key of the key/value pair.
    * @param {!string} value the value of the key/value pair.
    * @param {?string} storageType expected values localStorage and sessionStorage.
    * @returns {void}
    */
    const _saveItem = function (key, value) {

        if (key == null) throw new Error("The key argument is null or undefined.");
        if (typeof key !== "string") throw new Error("The key argument type must be string.");

        if (value == null) throw new Error("The value argument is null or undefined.");
        if (typeof value !== "string") throw new Error("The value argument type must be string.");

        if (!_isAvailable(storageType)) throw new Error(_errors.storageException);

        try {
            let storage = window[storageType ?? _storageType.defaultStorageType];

            storage.setItem(key, value);
        }
        catch (e) {
            if (e instanceof DOMException && (
                // everything except Firefox
                e.code === 22 ||
                // Firefox
                e.code === 1014 ||
                // test name field too, because code might not be present
                // everything except Firefox
                e.name === 'QuotaExceededError' ||
                // Firefox
                e.name === 'NS_ERROR_DOM_QUOTA_REACHED') &&
                // acknowledge QuotaExceededError only if there's something already stored
                (storage && storage.length !== 0) === true) throw new Error(_errors.storageException); throw e;
        }
    };

    // Error messages
    const _errors = {
        storageException: "Storage is not available or the storage maximum quota has been reached."
    };

    // Storage type object
    const _storageType = {
        defaultStorageType: "sessionStorage",
        sessionStorage: "sessionStorage",
        localStorage: "localStorage"
    };

    /**
     * Public members
     */
    this.SaveItem = _saveItem;
    this.RemoveItem = _removeItem;
    this.RetrieveItem = _retrieveItem;

}).apply(Aylos.Portal.Storage);
