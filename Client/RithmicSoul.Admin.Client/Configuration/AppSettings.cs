namespace RithmicSoul.Admin.Client.Configuration;

public class AppSettings
{
    public int SurveyQuestionsMaxCount { get; set; }
    public int SurveyQuestionsMinCount { get; set; }
    public string AuthoredSurveyCreationSuccessMessage { get; set; }
    public string AuthoredSurveyCreationFailureMessage { get; set; }
    public string RowHighlightBgColor { get; set; }
    public string RowHighlightBgColorTemplate { get; set; }
    public string DeleteSelectedItemMessage { get; set; }
    public string ItemCreatedSuccessMessageTemplate { get; set; }
    public string ItemCreatedFailureMessageTemplate { get; set; }
    public string ItemUpdatedSuccessMessageTemplate { get; set; }
    public string ItemsCreatedSuccessMessageTemplate { get; set; }
    public string ItemsCreatedFailureMessageTemplate { get; set; }
    public string RsDataGridRenderFragmentPropertyAttribute { get; set; }
    public string RsDataGridRenderFragmentTitleAttribute { get; set; }
    public string RsDataGridRenderFragmentGroupingAttribute { get; set; }
    public string ItemDeletedSuccessMessageTemplate { get; set; }
    public string ItemDeletedFailureMessageTemplate { get; set; }
    public string EditTableDialogMessageTemplate { get; set; }
    public string QuestionChoicesDeleteFailureMessage { get; set; }
    public string QuestionChoicesChooseableMultipleChoice { get; set; }
    public string QuestionChoicesChooseableDemographic { get; set; }
    public string QuestionChoicesDeleteCheckbox { get; set; }
    public string QuestionTypeNameCssVisibilityVisible { get; set; }
    public string QuestionTypeNameCssVisibilityHidden { get; set; }
}