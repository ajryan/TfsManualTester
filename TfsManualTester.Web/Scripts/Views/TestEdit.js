/*
 * Test/Edit
 *
 * TestCaseEditor
*/

TfsManualTester.TestCaseEditor = {
    Start: function(testCase) {
        var viewModel = new TfsManualTester.TestCaseEditor.ViewModel(testCase);
        TfsManualTester.TestCaseEditor.WireEvents(viewModel);
        ko.applyBindings(viewModel);
    }
};

TfsManualTester.TestCaseEditor.ViewModel = function(testCase) {
    var self = this;
    
    // this initial model should be creatable with fromJSON
    // but when the initial TestSteps is an empty list, ko
    // fromJSON doesn't create an observable array
    // also, need to ensure properties of objects in the array
    // are themselves observable...

    self.TeamProject = ko.observable(testCase.TeamProject);
    self.TestCaseId = ko.observable(testCase.TestCaseId);
    self.TestSteps = ko.observableArray(
        ko.utils.arrayMap(
            testCase.TestSteps,
            function(testStep) {
                return {
                    Id: ko.observable(testStep.Id),
                    Title: ko.observable(testStep.Title),
                    Expected: ko.observable(testStep.Expected)
                };
            }));
    self.ErrorMessage = ko.observable('');
    
    self.loadTest = function(event) {
        event.preventDefault();
        self.ErrorMessage('');
        
        // TODO: simpler post, see conery demo
        $.ajax({
            url: $(this).attr('data-url'),
            data: { teamProject: self.TeamProject(), testCaseId: self.TestCaseId() },
            type: 'POST',
            success: function (data) {
                if (data.Success === false) {
                    self.showError(data.ErrorMessage);
                }
                else {
                    self.TestSteps(data.TestSteps);
                    $('textarea.test-step').autoResize();
                }
            },
            error: function (xhr, status, error) {
                self.showError(status + ': ' + error);
            }
        });
    };

    self.showError = function(errorMessage) {
        self.TestSteps([]);
        self.ErrorMessage(errorMessage);
    };

    TfsManualTester.TestCaseEditor.CurrentViewModel = self;
};

TfsManualTester.TestCaseEditor.WireEvents = function (viewModel) {
    $('#loadTestCaseBtn')
        .live('click', viewModel.loadTest)
        .button()
        .ajaxStart(function () {
            $(this).button('loading');
        })
        .ajaxStop(function () {
            $(this).button('reset');
        });

    //$("div.test-step").live('click', function(event) {
    //});

    $('input.initial-focus:first').focus();
};