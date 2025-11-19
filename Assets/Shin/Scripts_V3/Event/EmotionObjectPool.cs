using System.Linq;
using UnityEngine;

//감정표현은 대화에서만 나오는것도 아니고 타임라인이나 이벤트로 재생하는게 나을것같음.
//그럼 씬의 전체 감정표현이벤트를 얻을 필요가 있을듯
public class EmotionObjectPool : MonoBehaviour
{
    [SerializeField] ParticleSystem[] emotions;
    [SerializeField] EmotionInvoker[] invokers;

    [SerializeField] GameObject[] objects;

    void Awake()
    {
        emotions = new ParticleSystem[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        { emotions[i] = transform.GetChild(i).GetComponent<ParticleSystem>(); }

        objects = GameObject.FindGameObjectsWithTag("EmotionInvoker").OrderBy(e => e.name).ToArray();
        invokers = new EmotionInvoker[objects.Length];
        for (int i = 0; i < invokers.Length; i++)
        { invokers[i] = objects[i].GetComponent<EmotionInvoker>(); }
    }

    /// <summary>
    /// Number에 맞는Invoker에 맞는 emotion을 넣어서 재생시키려함.
    /// 0번이 재생중이라면 1번을 넣고 .. 하는 식.  다 재생중이면 메세지 보냄.
    /// </summary>
    public void InvokeEmotion(int number)
    {
        //Invoker의 타입을 가져와야하는데 어떻게하지.. 그냥 public으로 하자 이건
        EmotionInvoker invoker = invokers[number];
        int type = invoker.emotionType;

        //使用可能なParticleを探す
        ParticleSystem particle = FindParticle(type);

        //Particleを渡す
        invoker.InvokeEmotion(particle);
    }

    ParticleSystem FindParticle(int type)
    {
        int firstType = type * 3;
        ParticleSystem[] particles = { emotions[firstType], emotions[firstType + 1], emotions[firstType + 2]};

        foreach( ParticleSystem particle in particles )
        {
            if (particle.isPlaying) { continue; }
            else { return particle; }
        }

        Debug.Log("남아있는 파티클이 존재하지 않는다 !");
        return null;
    }
}
