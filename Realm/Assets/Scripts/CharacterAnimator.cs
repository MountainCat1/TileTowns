using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAnimator : SpriteAnimator
{
    [SerializeField] private string framesLocation = "";
    private bool _animationsLoaded = false;

    public enum Animation
    {
        Idle, Walk
    }

    public void PlayAnimation(Animation animation, bool loop = true, int startFrame = 0)
    {
        Play(animation.ToString().ToLower(), loop, startFrame);
    }

    public void SetAnimation(Animation animation)
    {
        if(CurrentAnimation.Name != animation.ToString().ToLower())
        {
            PlayAnimation(animation);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        PlayAnimationOnStart = "idle";

        if (!_animationsLoaded)
            LoadAnimations();
    }

    private void LoadAnimations()
    {
        List<SpriteAnimation> loadedAnimations = new List<SpriteAnimation>();

        string basePath = $"Sprites/{framesLocation}";

        var assetNames = GameResources.GetAssetNames($"{basePath}").ToArray();
        
        loadedAnimations.Add(new SpriteAnimation()
        {
            Frames = GameResources.LoadSubAssets<Sprite>(basePath, assetNames.First(x => x.Contains("walk"))),
            Name = "walk",
            Type = SpriteAnimation.AnimationType.PingPong
        });
        
        loadedAnimations.Add(new SpriteAnimation()
        {
            Frames = GameResources.LoadSubAssets<Sprite>(basePath, assetNames.First(x => x.Contains("idle"))),
            Name = "idle"
        });
        
        Animations = loadedAnimations;
        _animationsLoaded = true;
    }
}