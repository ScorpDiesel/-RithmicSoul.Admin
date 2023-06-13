using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Models.Survey.Dtos;
using System.Linq;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Options;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class QuestionResponse : ComponentBase
{
    [Inject] IOptions<AppSettings> AppSettingsOptions { get; set; }
    [Parameter] public string QuestionText { get; set; }
    [Parameter] public int? QuestionNumber { get; set; }
    [Parameter] public Typo QuestionTextTypography { get; set; }
    [Parameter] public IEnumerable<AuthoredSurveyDto> Items { get; set; }

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
                    builder.OpenComponent(0, typeof(MudRating));
                    builder.AddAttribute(1, "MaxValue", 10);
                    builder.AddAttribute(2, "Size", Size.Large);
                    builder.AddAttribute(3, "Class", "ml-n2");
                    builder.AddAttribute(4, "SelectedValueChanged", EventCallback.Factory.Create<int>(this, (int value) => ValueChanged(value)));
                    builder.CloseComponent();
                    break;
                case "Open Ended":
                    builder.OpenComponent(0, typeof(MudTextField<string>));
                    builder.AddAttribute(1, "Lines", 10);
                    builder.AddAttribute(2, "Variant", Variant.Outlined);
                    builder.AddAttribute(3, "Class", "ml-n2");
                    builder.AddAttribute(4, "ValueChanged", EventCallback.Factory.Create<string>(this, (string value) => ValueChanged(value)));
                    builder.CloseComponent();
                    break;
                case "Yes/No":
                    var yesnoList = _appSettings.YesNoChoices.Split(',').ToList();
                    CreateRadioGroupFragment(builder, yesnoList, false);
                    break;
                case "Likert Scale":
                    var likertList = _appSettings.LikerScaleChoices.Split(',').ToList();
                    CreateRadioGroupFragment(builder, likertList, false);
                    break;
                case "Multiple Choice":
                case "Demographic":
                    CreateRadioGroupFragment(builder, choices.QuestionChoices, false);
                    break;
                case "Checkbox":
                    CreateCheckboxesFragment(builder, choices.QuestionChoices, false);
                    break;
            }
        };
    }

    private void CreateCheckboxesFragment(RenderTreeBuilder builder, List<string> checkboxLabels, bool isRow)
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
                childBuilder.AddAttribute(childSeq++, "CheckedChanged", EventCallback.Factory.Create<bool>(this, (bool value) => ValueChanged(label, value)));
                childBuilder.CloseComponent();
            }
        }));

        builder.CloseComponent();
    }

    void ValueChanged(string label, object value)
    {
        Console.WriteLine($"label: {label}, value: {value}");
    }

    void ValueChanged(int value)
    {
        Console.WriteLine($"label: {value}");
    }

    void ValueChanged(string value)
    {
        Console.WriteLine($"label: {value}");
    }


    private void CreateRadioGroupFragment(RenderTreeBuilder builder, List<string> radioLabels, bool isRow)
    {
        builder.OpenComponent<MudRadioGroup<bool>>(0);
        builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
        {
            childBuilder.OpenComponent<MudStack>(1);
            childBuilder.AddAttribute(2, "Row", isRow);

            childBuilder.AddAttribute(3, "ChildContent", (RenderFragment)(stackBuilder =>
            {
                var childSeq = 0;
                foreach (var label in radioLabels)
                {
                    stackBuilder.OpenComponent<MudRadio<bool>>(childSeq++);
                    stackBuilder.AddAttribute(childSeq++, "Option", true);
                    stackBuilder.AddAttribute(childSeq++, "Dense", true);
                    stackBuilder.AddAttribute(childSeq++, "Class", "mr-10");
                    stackBuilder.AddAttribute(childSeq++, "onchange", EventCallback.Factory.Create(this, value => ValueChanged(label)));
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

}
