using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class SpriteAnimator : MonoBehaviour
{
    [SerializeField] public float speedMultiplier = 1f;
    [HideInInspector] public string PlayAnimationOnStart { get; set; }

    [HideInInspector] private SpriteRenderer _spriteRenderer;

    [HideInInspector] public List<SpriteAnimation> Animations { get; set; }
    [HideInInspector] public SpriteAnimation CurrentAnimation { get; set; }
    [HideInInspector] public bool Playing { get; set; }
    [HideInInspector] public bool Flipped { get => _spriteRenderer.flipX; set => _spriteRenderer.flipX = value; }

    [HideInInspector] private int currentFrame = 0;
    [HideInInspector] private bool loop = true;

    public int RendererSortingOrder { get => _spriteRenderer.sortingOrder; set => _spriteRenderer.sortingOrder = value; }

    protected virtual void Awake()
    {
        if (!_spriteRenderer)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnEnable()
    {
        if (PlayAnimationOnStart != "")
            Play(PlayAnimationOnStart);
    }
    protected virtual void OnDisable()
    {
        Playing = false;
        CurrentAnimation = null;
    }

    public void Play(Enum @enum)
    {
        Play(@enum.ToString());
    }
    
    public void Play(string name, bool loop = true, int startFrame = 0)
    {
        SpriteAnimation animation = GetAnimation(name);
        if (animation != null)
        {
            if (animation != CurrentAnimation)
            {
                ForcePlay(name, loop, startFrame);
            }
        }
        else
        {
            Debug.LogWarning("could not find animation: " + name);
        }
    }
    public void ForcePlay(string name, bool loop = true, int startFrame = 0)
    {
        SpriteAnimation animation = GetAnimation(name);
        if (animation != null)
        {
            this.loop = loop;
            CurrentAnimation = animation;
            Playing = true;
            currentFrame = startFrame;
            _spriteRenderer.sprite = animation.Frames[currentFrame];
            StopAllCoroutines();
            StartCoroutine(PlayAnimation(CurrentAnimation));
        }
    }

    public SpriteAnimation GetAnimation(string name)
    {
        var spriteAnimation = Animations.FirstOrDefault(x 
            => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));

        if (spriteAnimation is null)
        {
            Debug.LogError($"Animation {name} could not be found!");
            throw new NullReferenceException();
        }

        return spriteAnimation;
    }

    IEnumerator PlayAnimation(SpriteAnimation animation)
    {
        float timer = 0f;
        bool direction = true;
        

        while (loop || currentFrame < animation.Frames.Length - 1)
        {
            var delay = animation.Time / speedMultiplier;
            
            while (timer < delay) // wait to match fps
            {
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            while (timer > delay) // display frames
            {
                timer -= delay;

                NextFrame(animation, ref direction);
            }

            _spriteRenderer.sprite = animation.Frames[currentFrame];
        }

        CurrentAnimation = null;
        Playing = false;
    }

    void NextFrame(SpriteAnimation animation, ref bool direction)
    {
        if (!loop)
        {
            currentFrame++;

            if (currentFrame >= animation.Frames.Length)
            {
                currentFrame = animation.Frames.Length - 1;
            }
        }


        switch (animation.Type)
        {
            case SpriteAnimation.AnimationType.Normal:
                currentFrame++;

                if (currentFrame >= animation.Frames.Length)
                {
                    currentFrame = 0;
                }
                break;
            case SpriteAnimation.AnimationType.PingPong:
                if (direction)
                {
                    currentFrame += 1;

                    if (currentFrame >= animation.Frames.Length - 1)
                    {
                        direction = false;
                    }
                }
                else
                {
                    currentFrame -= 1;

                    if (currentFrame <= 0)
                    {
                        direction = true;
                    }
                }
                break;

            default:
                throw new NotImplementedException();
        }
    }
}

[System.Serializable]
public class SpriteAnimation
{
    public enum AnimationType
    {
        Normal, PingPong, PingPongFull, Reversed
    }

    public string Name { get; set; }
    public float Speed { get; private set; } = 1;
    public float Time { get => 1f / Speed; }
    public AnimationType Type { get; set; }
    public Sprite[] Frames { get; set; }


    public SpriteAnimation(string name, float speed, Sprite[] frames)
    {
        Name = name;
        Speed = speed;
        Frames = frames;
    }

    public SpriteAnimation()
    {
    }
}