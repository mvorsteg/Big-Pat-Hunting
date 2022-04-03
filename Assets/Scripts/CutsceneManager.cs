using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector introCutscene;
    [SerializeField]
    private PlayableDirector nightCutscene;

    private PlayableDirector currentCutscene;
    private PlayerControls controls;
    private Player player;
    
    private void Awake()
    {
        player = FindObjectOfType<Player>();


        controls = new PlayerControls();   

        controls.AnyKey.AnyKey.performed += ctx => AnyKeyPressed();
    }

    private void Start()
    {
        //PlayIntroCutscene();   
        //PlayNightCutscene();
    }

    private IEnumerator PlayCutcene(PlayableDirector cutscene, bool interruptible = false, float delayBeforeInterruptible = 0f)
    {
        currentCutscene = cutscene;
        player.gameObject.SetActive(false);
        cutscene.gameObject.SetActive(true);
        
        cutscene.Play();
        
        if (interruptible)
        {
            yield return new WaitForSeconds(delayBeforeInterruptible);
            controls.Enable();
        }

        yield return new WaitUntil(() => cutscene.state != PlayState.Playing);

        controls.AnyKey.Disable();
        cutscene.gameObject.SetActive(false);
        player.gameObject.SetActive(true);

        //QuestManager.NextDay();
    }

    public void AnyKeyPressed()
    {
        currentCutscene.Stop();
    }

    public void PlayIntroCutscene()
    {
        StartCoroutine(PlayCutcene(introCutscene, false));
    }

    public void PlayNightCutscene()
    {
        StartCoroutine(PlayCutcene(nightCutscene, true));
    }
}