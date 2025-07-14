using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "DriftRush/Level Data")]
public class LevelData : ScriptableObject {
    [Header("Level Info")]
    [SerializeField] private string levelName;
    [SerializeField] private string sceneName;
    [SerializeField] private Sprite levelImage;

    [Header("Game Settings")]
    [SerializeField] private int targetCoins = 10;
    [SerializeField] private float timeLimit = 60f;

    public string LevelName => levelName;
    public string SceneName => sceneName;
    public Sprite LevelImage => levelImage;
    public int TargetCoins => targetCoins;
    public float TimeLimit => timeLimit;
}