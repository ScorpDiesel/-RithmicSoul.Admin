using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Models.Survey.Dtos;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Options;
using RithmicSoul.Admin.Core.Models;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class RsQuestionResponse : ComponentBase
{
    [Inject] private IOptions<AppSettings> AppSettingsOptions { get; set; }
    [Parameter] public string QuestionText { get; set; }
    [Parameter] public int QuestionNumber { get; set; }
    [Parameter] public Typo QuestionTextTypography { get; set; }
    [Parameter] public IEnumerable<AuthoredSurveyDto> Items { get; set; }
    [Parameter] public EventCallback<QuestionResponseObject> OnValueChanged { get; set; }

    private IEnumerable<QuestionChoiceObject> _choices;
    private AppSettings _appSettings;
    private string _questionNumberText;
    private string _questionClass;
    private List<QuestionResponseObject> _questionResponses = new();

    protected override void OnInitialized()
    {
        Initialize();
    }

    private void Initialize()
    {
        _appSettings = AppSettingsOptions.Value;
        _choices = Items.GroupBy(q => q.QuestionText)
            .Select(g => new QuestionChoiceObject{ QuestionType = g.Select(x => x.QuestionTypeName).First(),
                QuestionId = g.Select(x => x.QuestionId).First(),
                QuestionChoices = g.Select(x => x.ChoiceText).ToList()});
    }

    private RenderFragment CreateRenderFragment(QuestionChoiceObject choices)
    {
        return builder =>
        {
            switch (choices.QuestionType)
            {
                case "Rating":
                    CreateRatingFragment(choices, builder);
                    break;
                case "Open Ended":
                    CreateTextFieldFragment(choices, builder);
                    break;
                case "Yes/No":
                    var yesnoList = _appSettings.YesNoChoices.Split(',').ToList();
                    CreateRadioGroupFragment(choices, builder, yesnoList, false);
                    break;
                case "Likert Scale":
                    var likertList = _appSettings.LikerScaleChoices.Split(',').ToList();
                    CreateRadioGroupFragment(choices, builder, likertList, false);
                    break;
                case "Single Choice":
                case "Demographic":
                    CreateRadioGroupFragment(choices, builder, choices.QuestionChoices, false);
                    break;
                case "Multiple Choice":
                    CreateCheckboxesFragment(choices, builder, choices.QuestionChoices, false);
                    break;
                case "Rank":
                    break;
                case "Image":
                    break;
            }
        };
    }

    //private void CreateRankFragment(string questionType, RenderTreeBuilder builder)
    //{
    //    builder.OpenComponent(0, typeof());
    //}

    private void CreateTextFieldFragment(QuestionChoiceObject choices, RenderTreeBuilder builder)
    {
        builder.OpenComponent(0, typeof(MudTextField<string>));
        builder.AddAttribute(1, "Lines", 10);
        builder.AddAttribute(2, "Variant", Variant.Outlined);
        builder.AddAttribute(3, "Class", "ml-n2");
        builder.AddAttribute(5, "ValueChanged",
            EventCallback.Factory.Create<string>(this, value => ValueChangedAsync(choices, value)));
        builder.CloseComponent();
    }

    private void CreateRatingFragment(QuestionChoiceObject choices, RenderTreeBuilder builder)
    {
        builder.OpenComponent(0, typeof(MudRating));
        builder.AddAttribute(1, "MaxValue", 5);
        builder.AddAttribute(2, "Size", Size.Large);
        builder.AddAttribute(3, "Class", "ml-n2");
        builder.AddAttribute(5, "SelectedValueChanged",
            EventCallback.Factory.Create<int>(this, value => ValueChangedAsync(choices, value)));
        builder.CloseComponent();
    }

    private void CreateCheckboxesFragment(QuestionChoiceObject choices, RenderTreeBuilder builder, List<string> checkboxLabels, bool isRow)
    {
        builder.OpenComponent<MudStack>(0);
        builder.AddAttribute(1, "Row", isRow);
        builder.AddAttribute(2, "Class", "ml-n3");
        builder.AddAttribute(3, "ChildContent", (RenderFragment)(childBuilder =>
        {
            foreach (var label in checkboxLabels)
            {
                var childSeq = 0;
                childBuilder.OpenComponent<MudCheckBox<bool>>(childSeq++);
                childBuilder.AddAttribute(childSeq++, "UnCheckedColor", Color.Default);
                childBuilder.AddAttribute(childSeq++, "Dense", true);
                childBuilder.AddAttribute(childSeq++, "Label", label);
                childBuilder.AddAttribute(childSeq++, "CheckedChanged", EventCallback.Factory.Create<bool>(this, value => ValueChangedAsync(choices, label, value)));
                childBuilder.CloseComponent();
            }
        }));

        builder.CloseComponent();
    }

    private void CreateRadioGroupFragment(QuestionChoiceObject choices, RenderTreeBuilder builder, List<string> radioLabels, bool isRow)
    {
        builder.OpenComponent<MudRadioGroup<string>>(0);
        builder.AddAttribute(2, "ChildContent", (RenderFragment)(childBuilder =>
        {
            childBuilder.OpenComponent<MudStack>(3);
            childBuilder.AddAttribute(4, "Row", isRow);
            childBuilder.AddAttribute(5, "ChildContent", (RenderFragment)(stackBuilder =>
            {
                var childSeq = 0;
                foreach (var label in radioLabels)
                {
                    stackBuilder.OpenComponent<MudRadio<string>>(childSeq++);
                    stackBuilder.AddAttribute(childSeq++, "Option", label);
                    stackBuilder.AddAttribute(childSeq++, "Dense", true);
                    stackBuilder.AddAttribute(childSeq++, "Class", "mr-10");
                    stackBuilder.AddAttribute(childSeq++, "onchange", EventCallback.Factory.Create(this, value => ValueChangedAsync(choices, label)));
                    stackBuilder.AddAttribute(childSeq++, "ChildContent", (RenderFragment)(builder2 => {
                        builder2.AddContent(childSeq++, label);
                    }));
                    stackBuilder.CloseComponent();
                }
            }));

            childBuilder.CloseComponent();
        }));
        builder.CloseComponent();
    }

    private async Task ValueChangedAsync(QuestionChoiceObject choices, object value, bool? isSelected = null)
    {
        var response = new QuestionResponseObject
        {
            QuestionType = choices.QuestionType,
            QuestionText = choices.QuestionText,
            QuestionId = choices.QuestionId,
            IsSelected = isSelected ?? false,
            Responses = new List<string>{ value.ToString() }
        };

        await OnValueChanged.InvokeAsync(response);
    }
}
