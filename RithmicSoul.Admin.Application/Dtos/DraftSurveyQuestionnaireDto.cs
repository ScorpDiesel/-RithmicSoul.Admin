using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RithmicSoul.Admin.Application.Dtos;

public class DraftSurveyQuestionnaireDto : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private int _surveyQuestionId;
    private string _surveyQuestionText;
    private string _surveyName;
    public int QuestionnaireId { get; set; }

    public int SurveyQuestionId
    {
        get => _surveyQuestionId;
        set => SetField(ref _surveyQuestionId, value);
    }

    public int SurveyId { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }

    public string SurveyQuestionText
    {
        get => _surveyQuestionText;
        set => SetField(ref _surveyQuestionText, value);
    }

    public string SurveyName
    {
        get => _surveyName;
        set => SetField(ref _surveyName, value);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}