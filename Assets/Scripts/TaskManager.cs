using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public class Task
    {
        public string Description { get; private set; }
        public bool IsCompleted { get; private set; }
        public TextMeshProUGUI TaskText { get; private set; }

        public Task(string description, TextMeshProUGUI taskText)
        {
            Description = description;
            TaskText = taskText;
            Reset();
        }

        public void Complete()
        {
            IsCompleted = true;
            TaskText.color = Color.green;
            TaskText.text = $"<s>{Description}</s>";
        }

        public void Reset()
        {
            IsCompleted = false;
            TaskText.color = Color.red;
            TaskText.text = Description;
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription;
            TaskText.text = Description;
        }
    }

    public bool isClothesTaskActive = true;
    public bool wineTaskDone = false;
    public bool fruitTaskDone = false;
    public bool musicTaskDone = false;
    public bool lastLaundryActive = false;

    private const float CLOTHES_TASK_CHECK_INTERVAL = 0.5f;
    private const string FINAL_REMINDER_MESSAGE = "Looks like I missed a dirty laundry.";

    [SerializeField] private TextMeshProUGUI clothesTaskText;
    [SerializeField] private TextMeshProUGUI wineTaskText;
    [SerializeField] private TextMeshProUGUI fruitTaskText;
    [SerializeField] private TextMeshProUGUI musicTaskText;
    [SerializeField] private GameObject lastLaundry;
    [SerializeField] private List<GameObject> clothesObjects;
    [SerializeField] private CaptionTextTyper captionTextTyper;

    private List<Task> tasks = new List<Task>();

    void Start()
    {
        InitializeTasks();
        StartCoroutine(CheckClothesTaskPeriodically());
    }

    private void InitializeTasks()
    {
        tasks = new List<Task>
        {
            new Task("• Put clothes into laundry basket.", clothesTaskText),
            new Task("• Pour wine into a glass.", wineTaskText),
            new Task("• Prepare a fruit plate.", fruitTaskText),
            new Task("• Play music on the laptop.", musicTaskText)
        };
    }

    void Update()
    {
        if (wineTaskDone && !tasks[1].IsCompleted) 
            tasks[1].Complete();
        if (fruitTaskDone && !tasks[2].IsCompleted) 
            tasks[2].Complete();
        if (musicTaskDone && !tasks[3].IsCompleted) 
            tasks[3].Complete();

        CheckAllTasksCompleted();
    }

    private void CheckAllTasksCompleted()
    {
        if (tasks.All(t => t.IsCompleted) && !lastLaundryActive)
            AllTasksEnded();
    }

    private IEnumerator CheckClothesTaskPeriodically()
    {
        while (isClothesTaskActive)
        {
            UpdateClothesTask();
            yield return new WaitForSeconds(CLOTHES_TASK_CHECK_INTERVAL);
        }
    }

    private void UpdateClothesTask()
    {
        clothesObjects = clothesObjects.Where(cloth => cloth != null).ToList();
        if (clothesObjects.Count == 0)
        {
            tasks[0].Complete();
            isClothesTaskActive = false;
        }
        else
        {
            tasks[0].UpdateDescription($"• Put {clothesObjects.Count} clothes into laundry basket.");
        }
    }

    private void AllTasksEnded()
    {
        captionTextTyper.StartType(FINAL_REMINDER_MESSAGE, true);
        lastLaundry.SetActive(true);
        lastLaundryActive = true;
    }
}
