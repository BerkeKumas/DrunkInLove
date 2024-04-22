using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    public class Task
    {
        public string Description;
        public bool IsCompleted;
        public TextMeshProUGUI TaskText;

        public Task(string description, TextMeshProUGUI taskText)
        {
            Description = description;
            IsCompleted = false;
            TaskText = taskText;
            TaskText.color = Color.red;  // Baþlangýçta görevlerin metin rengi kýrmýzý
            TaskText.text = description;  // Görevin açýklamasýný baþlangýçta ayarla
        }
    }

    public List<Task> tasks = new List<Task>();
    public List<GameObject> clothesObjects;

    public TextMeshProUGUI clothesTaskText;
    public TextMeshProUGUI wineTaskText;
    public TextMeshProUGUI fruitTaskText;
    public TextMeshProUGUI musicTaskText;

    public bool isClothesTaskActive = true;
    public bool wineTaskDone = false;
    public bool fruitTaskDone = false;
    public bool musicTaskDone = false;

    void Start()
    {
        tasks.Add(new Task("Clothes Count: Clean all clothes objects.", clothesTaskText));
        tasks.Add(new Task("Pour wine into a glass.", wineTaskText));
        tasks.Add(new Task("Prepare a fruit plate.", fruitTaskText));
        tasks.Add(new Task("Play music on the laptop.", musicTaskText));

        StartCoroutine(CheckClothesTaskPeriodically());
    }

    IEnumerator CheckClothesTaskPeriodically()
    {
        while (isClothesTaskActive)
        {
            UpdateClothesTask();  // Kýyafet görevini güncelle
            yield return new WaitForSeconds(0.5f);  // Her 0.5 saniyede bir kontrol et
        }
    }

    void Update()
    {
        if (wineTaskDone)
            CompleteTask(1);

        if (fruitTaskDone)
            CompleteTask(2);

        if (musicTaskDone)
            CompleteTask(3);
        if (!isClothesTaskActive && wineTaskDone && fruitTaskDone && musicTaskDone)
        {
            SceneManager.LoadScene(2);
        }
    }

    public void DestroyClothes(GameObject cloth)
    {
        if (clothesObjects.Contains(cloth))
        {
            clothesObjects.Remove(cloth);
            Destroy(cloth);
        }
    }

    void UpdateClothesTask()
    {
        clothesObjects = clothesObjects.Where(cloth => cloth != null).ToList(); // Null olmayanlarý temizle
        int clothesCount = clothesObjects.Count;
        tasks[0].Description = $"Clothes Count: Clean all {clothesCount} clothes objects.";
        tasks[0].TaskText.text = tasks[0].Description;

        if (clothesCount == 0)
        {
            CompleteTask(0);
            isClothesTaskActive = false; // Görev tamamlandý, coroutine'i durdur
        }
    }

    void CompleteTask(int index)
    {
        Task task = tasks[index];
        if (!task.IsCompleted)
        {
            task.IsCompleted = true;
            task.TaskText.color = Color.green; // Görev tamamlandýðýnda metin rengi yeþil olacak
        }
    }
}
