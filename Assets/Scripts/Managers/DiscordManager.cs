using UnityEngine;

public class DiscordManager : MonoBehaviour {
#if UNITY_STANDALONE_WIN
    [SerializeField] long clientId;
    private
    Discord.Discord discord;

    private void Start() {
        discord = new Discord.Discord(clientId, (ulong)Discord.CreateFlags.NoRequireDiscord);
        ChangeActivity();
    }

    void Update() {
        discord?.RunCallbacks();
    }

    void OnApplicationQuit() {
        discord?.Dispose();
    }

    private void ChangeActivity() {
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity {
            State = "Drifting"
        };

        activityManager.UpdateActivity(activity, (result) => {
            if (result == Discord.Result.Ok) {
                Debug.Log("Discord Activity Updated Successfully");
            }
        });
    }
#endif
}
