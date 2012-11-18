TfsManualTester.Login = {
    // TODO: this should take a JSON model from the controller
    Start : function (returnUrl, tfsUrl, userName) {
        ko.validation.configure({
            registerExtenders: true,
            messagesOnModified: true,
            insertMessages: true,
            parseInputAttributes: true,
            messageTemplate: null
        });
        
        var viewModel = new TfsManualTester.ViewModel(returnUrl, tfsUrl, userName);
        TfsManualTester.WireEvents(viewModel);
        ko.applyBindings(viewModel);
    }
};

TfsManualTester.ViewModel = function(returnUrl, tfsUrl, userName) {
    var self = this;
    
    // TODO: caps for observables
    self.tfsUrl = ko.observable(tfsUrl).extend({ required: true });
    self.userName = ko.observable(userName).extend({ required: true });
    self.password = ko.observable('').extend({ required: true });
    self.errorMessage = ko.observable('');
    self.errors = ko.validation.group(self);
    
    self.login = function(event) {
        event.preventDefault();

        if (self.errors().length !== 0) {
            self.errors.showAllMessages();
            return;
        }
        self.errorMessage('');

        // TODO: just serialize the form?
        //       in that case, is there any use for ko?
        var formData = {
            tfsUrl: self.tfsUrl(),
            serviceIdUsername: self.userName(),
            serviceIdPassword: self.password()
        };

        // TODO: simpler post per conery example
        $.ajax({
            url: $(this).attr('data-url'),
            data: formData,
            type: 'POST',
            success: self.loginSuccess,
            error: self.loginError
        });
    };
    
    self.loginSuccess = function(data) {
        if (data.Success === false) {
            self.showErrorMessage(data.ErrorMessage);
        }
        else {
            var redirect = returnUrl || data.RedirectUrl;
            window.location = redirect;
        }
    };

    self.loginError = function(xhr, status, error) {
        self.showErrorMessage(status + ': ' + error);
    };
    
    self.showErrorMessage = function(message) {
        $('#submit').button('reset');
        self.errorMessage(message);
    };
};

TfsManualTester.WireEvents = function(viewModel) {

    $('#submit')
        .live('click', viewModel.login)
        .button()
        .ajaxStart(function () {
            $(this).button('loading');
        });

    $('input.initial-focus:first').focus();
};