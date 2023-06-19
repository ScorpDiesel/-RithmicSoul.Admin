using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using RithmicSoul.Admin.Application.Interfaces;

namespace RithmicSoul.Admin.Application.Dtos;

public class DraftSurveyDto : IDirty
{
    private string _surveyName;
    private string _surveyDescription;
    private string _surveyTypeName;
    private bool _isDirty;

    public bool IsDirty => _isDirty;

    public void SetDirty(bool value)
    {
        _isDirty = value;
    }

    public DraftSurveyDto()
    {
        SurveyQuestions.CollectionChanged += OnSurveyQuestionsChanged;
    }

    public int SurveyId { get; set; }

    public string SurveyName
    {
        get => _surveyName;
        set
        {
            _surveyName = value;
            _isDirty = true;
        }
    }

    public string SurveyDescription
    {
        get => _surveyDescription;
        set
        {
            _surveyDescription = value;
            _isDirty = true;
        }
    }

    public int SurveyTypeId { get; set; }

    public string SurveyTypeName
    {
        get => _surveyTypeName;
        set
        {
            _surveyTypeName = value; 
            _isDirty = true;
        }
    }

    public string SurveyQuestionText { get; set; }

    public int SurveyQuestionId { get; set; }

    public DateTime? DateCreated { get; set; }

    public List<int> SurveyQuestionIds { get; set; } = new();

    public ObservableCollection<DraftSurveyQuestionnaireDto> SurveyQuestions { get; set; } = new();
    
    private void OnSurveyQuestionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (DraftSurveyQuestionnaireDto newItem in e.NewItems)
            {
                newItem.PropertyChanged += OnItemPropertyChanged;
            }
        }

        if (e.OldItems != null)
        {
            foreach (DraftSurveyQuestionnaireDto oldItem in e.OldItems)
            {

                oldItem.PropertyChanged -= OnItemPropertyChanged;
            }
        }
    }

    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e) => _isDirty = true;
}