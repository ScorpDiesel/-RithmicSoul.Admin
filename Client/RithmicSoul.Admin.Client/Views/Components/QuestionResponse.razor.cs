using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Models.Survey.Dtos;
using System.Linq;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Options;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Admin.Client.Pages.Surveys;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class QuestionResponse : ComponentBase
{
    [Inject] private IOptions<AppSettings> AppSettingsOptions { get; set; }
    [Parameter] public string QuestionText { get; set; }
    [Parameter] public int? QuestionNumber { get; set; }
    [Parameter] public Typo QuestionTextTypography { get; set; }
    [Parameter] public IEnumerable<AuthoredSurveyDto> Items { get; set; }
    [Parameter] public EventCallback<QuestionResponseObject> OnValueChanged { get; set; }
    [Parameter] public ProductSurvey Parent { get; set; }

    private IEnumerable<(string QuestionType, List<string> QuestionChoices)> _choices;
    private AppSettings _appSettings;
    private string _questionNumberText;
    private string _questionClass;



    protected override void OnInitialized()
    {
        SetFields();
    }

    private void SetFields()
    {
        Parent.NewPage += UpdateValue;
        _appSettings = AppSettingsOptions.Value;
        _choices = Items.Where(c => c.QuestionText == QuestionText)
            .GroupBy(q => q.QuestionText)
            .Select(g => (QuestionType: g.Select(x => x.QuestionTypeName).First(),
                QuestionChoices: g.Select(x => x.ChoiceText).ToList()));
        
        if (QuestionNumber is null)
        {
            _questionNumberText = "";
            _questionClass = "";
        }
        else
        {
            _questionNumberText = $"Question #{QuestionNumber}";
            _questionClass = "mr-5";
        }
    }

    private RenderFragment CreateRenderFragment((string QuestionType, List<string> QuestionChoices) choices)
    {
        return builder =>
        {
            switch (choices.QuestionType)
            {
                case "Rating":
                    CreateRatingFragment(choices.QuestionType, builder);
                    break;
                case "Open Ended":
                    CreateTextFieldFragment(choices.QuestionType, builder);
                    break;
                case "Yes/No":
                    var yesnoList = _appSettings.YesNoChoices.Split(',').ToList();
                    CreateRadioGroupFragment(choices.QuestionType, builder, yesnoList, false);
                    break;
                case "Likert Scale":
                    var likertList = _appSettings.LikerScaleChoices.Split(',').ToList();
                    CreateRadioGroupFragment(choices.QuestionType, builder, likertList, false);
                    break;
                case "Multiple Choice":
                case "Demographic":
                    CreateRadioGroupFragment(choices.QuestionType, builder, choices.QuestionChoices, false);
                    break;
                case "Checkbox":
                    CreateCheckboxesFragment(choices.QuestionType, builder, choices.QuestionChoices, false);
                    break;
            }
        };
    }

    private void CreateTextFieldFragment(string questionType, RenderTreeBuilder builder)
    {
        builder.OpenComponent(0, typeof(MudTextField<string>));
        builder.AddAttribute(1, "Lines", 10);
        builder.AddAttribute(2, "Variant", Variant.Outlined);
        builder.AddAttribute(3, "Class", "ml-n2");
        builder.AddAttribute(4, "ValueChanged",
            EventCallback.Factory.Create<string>(this, value => ValueChangedAsync(questionType, value)));
        builder.CloseComponent();
    }

    private void CreateRatingFragment(string questionType, RenderTreeBuilder builder)
    {
        builder.OpenComponent(0, typeof(MudRating));
        builder.AddAttribute(1, "MaxValue", 10);
        builder.AddAttribute(2, "Size", Size.Large);
        builder.AddAttribute(3, "Class", "ml-n2");
        builder.AddAttribute(4, "SelectedValueChanged",
            EventCallback.Factory.Create<int>(this, value => ValueChangedAsync(questionType, value)));
        builder.CloseComponent();
    }

    private void CreateCheckboxesFragment(string questionType, RenderTreeBuilder builder, List<string> checkboxLabels, bool isRow)
    {
        builder.OpenComponent<MudStack>(0);
        builder.AddAttribute(1, "Row", isRow);
        builder.AddAttribute(1, "Class", "ml-n3");
        builder.AddAttribute(2, "ChildContent", (RenderFragment)(childBuilder =>
        {
            foreach (var label in checkboxLabels)
            {
                var childSeq = 0;
                childBuilder.OpenComponent<MudCheckBox<bool>>(childSeq++);
                childBuilder.AddAttribute(childSeq++, "UnCheckedColor", Color.Default);
                childBuilder.AddAttribute(childSeq++, "Dense", true);
                childBuilder.AddAttribute(childSeq++, "Label", label);
                childBuilder.AddAttribute(childSeq++, "CheckedChanged", EventCallback.Factory.Create<bool>(this, (bool value) => ValueChangedAsync(questionType, label, value)));
                childBuilder.CloseComponent();
            }
        }));

        builder.CloseComponent();
    }


    private void CreateRadioGroupFragment(string questionType, RenderTreeBuilder builder, List<string> radioLabels, bool isRow)
    {
        builder.OpenComponent<MudRadioGroup<string>>(0);
        builder.AddAttribute(2, "ChildContent", (RenderFragment)(childBuilder =>
        {
            childBuilder.OpenComponent<MudStack>(1);
            childBuilder.AddAttribute(2, "Row", isRow);

            childBuilder.AddAttribute(3, "ChildContent", (RenderFragment)(stackBuilder =>
            {
                var childSeq = 0;
                foreach (var label in radioLabels)
                {
                    stackBuilder.OpenComponent<MudRadio<string>>(childSeq++);
                    stackBuilder.AddAttribute(childSeq++, "Option", label);
                    stackBuilder.AddAttribute(childSeq++, "Dense", true);
                    stackBuilder.AddAttribute(childSeq++, "Class", "mr-10");
                    stackBuilder.AddAttribute(childSeq++, "onchange", EventCallback.Factory.Create(this, value => ValueChangedAsync(questionType, label)));
                    stackBuilder.AddAttribute(childSeq++, "ChildContent", (RenderFragment)(builder2 => {
                        builder2.AddContent(childSeq, label);
                    }));
                    stackBuilder.CloseComponent();
                }
            }));

            childBuilder.CloseComponent();
        }));

        builder.CloseComponent();
    }

    private async Task ValueChangedAsync(string questionType, string label, bool isSelected)
    {
        var response = new QuestionResponseObject
        {
            QuestionType = questionType,
            QuestionText = QuestionText,
            Response = label,
            isSelected = isSelected
        };

        await OnValueChanged.InvokeAsync(response);
    }

    private async Task ValueChangedAsync(string questionType, int value)
    {
        var response = new QuestionResponseObject
        {
            QuestionType = questionType,
            QuestionText = QuestionText,
            Response = value
        };

        await OnValueChanged.InvokeAsync(response);
    }

    private async Task ValueChangedAsync(string questionType, object value)
    {
        var response = new QuestionResponseObject
        {
            QuestionType = questionType,
            QuestionText = QuestionText,
            Response = value
        };

        await OnValueChanged.InvokeAsync(response);
    }

    private void UpdateValue(object? sender, (string questionType, List<object> responses) e)
    {
        var questionType = e.questionType;
        var response = e.responses;
    }
}
