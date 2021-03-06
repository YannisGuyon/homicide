﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public CameraManager camera_manager;
    public SceneManager scene_manager;
    public UiManager ui_manager;
    public AudioSource audio_source;

    private Phase phase;
    private float elapsed_time;
    public Fight fight;
    public Home home;

    public AudioClip menuMusic;
    public AudioClip cineMusic;
    public AudioClip mainTheme;
    public AudioClip introBattleTheme;
    public AudioClip battleTheme;
    public AudioClip endingCinematic;
    public AudioClip endingTheme;

    public Animator[] animators;
    public Transform home_transform;
    public Transform cottage_transform;
    public Transform factory_transform;
    public Transform skymachin_transform;
    public Transform catedral_transform;

    private LinkedList<Fx> fxs = new LinkedList<Fx>();
    public SoundGenerator soundGenerator;

    void Start()
    {
        if (cottage_transform == null) cottage_transform = GameObject.Find("Scene/scene/Cube").transform;
        if (factory_transform == null) factory_transform = GameObject.Find("Scene/scene/Cylinder_001").transform;
        if (skymachin_transform == null) skymachin_transform = GameObject.Find("Scene/scene/Plane_003").transform;
        if (catedral_transform == null) catedral_transform = GameObject.Find("Scene/scene/Cube_008").transform;

        camera_manager.shader_camera.material = Instantiate(camera_manager.shader_camera.material);
        phase = new PhaseWorldIntro();
        phase.main = this;
        phase.Init();
        elapsed_time = 0;
        ui_manager.Init();
        this.home = new Home();
        fight = new FightCottage(new Cottage(), this.home, soundGenerator);
    }

    void Update()
    {
        // Debug
        if (Input.GetKey(KeyCode.U))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                Time.timeScale = 0;
            else
                Time.timeScale = Mathf.Clamp(Time.timeScale - Time.unscaledDeltaTime, 0, 5);
            Debug.Log("Time.timeScale = " + Time.timeScale);
        }
        if (Input.GetKey(KeyCode.I))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                Time.timeScale = 1;
            else
                Time.timeScale = Mathf.Clamp(Time.timeScale + Time.unscaledDeltaTime, 0, 5);
            Debug.Log("Time.timeScale = " + Time.timeScale);
        }
        
        if (phase != null) phase.Update(elapsed_time);
        if (phase.IsDone(elapsed_time))
        {
            elapsed_time = 0;
            if (phase.GetType() == typeof(PhaseFight) && fight != null && fight.home.life <= 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver", UnityEngine.SceneManagement.LoadSceneMode.Single);
                // Game over
                Debug.Log("Game Over");
            }
            else
            {
                phase = phase.GetNextPhase();
                if (phase != null)
                {
                    phase.main = this;
                    phase.Init();
                }
            }
        }
        ui_manager.UpdateUi(this);
        for (LinkedList<Fx>.Enumerator it = fxs.GetEnumerator(); it.MoveNext();)
            it.Current.Update();
        while (fxs.Count > 0 && fxs.First.Value.IsDone()) fxs.RemoveFirst();
        elapsed_time += Time.deltaTime;
        if (phase == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("End", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }

    public void Add(Fx fx)
    {
        fx.main = this;
        fx.Init();
        fxs.AddLast(fx);
    }
}

public abstract class Fx
{
    public Main main;
    public abstract void Init();
    public abstract void Update();
    public abstract bool IsDone();
}

public abstract class FxDuration : Fx
{
    protected float elapsed_time;
    public override void Init()
    {
        elapsed_time = 0;
    }
    public override void Update()
    {
        elapsed_time += Time.deltaTime;
    }
    public override bool IsDone()
    {
        return elapsed_time >= GetDuration();
    }
    public abstract float GetDuration();
    protected float GetProgress()
    {
        return Mathf.Clamp01(elapsed_time / GetDuration());
    }
}

public abstract class FxTransform : FxDuration
{
    protected Transform transform;
    protected Vector3 local_position;
    protected Quaternion local_rotation;
    protected Vector3 local_scale;

    public FxTransform(Transform mesh_transform)
    {
        transform = mesh_transform;
        local_position = transform.localPosition;
        local_rotation = transform.localRotation;
        local_scale = transform.localScale;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() == 1)
        {
            transform.localPosition = local_position;
            transform.localRotation = local_rotation;
            transform.localScale = local_scale;
        }
    }
}

public class FxRollout : FxTransform
{
    public FxRollout(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.45f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {
            transform.localRotation = local_rotation * Quaternion.AngleAxis(GetProgress() * 360, local_rotation * Vector3.left);
        }
    }
}

public class FxExplodingChimney : FxTransform
{
    public FxExplodingChimney(Transform mesh_transform) : base(mesh_transform.GetChild(3))
    {
    }
    public override float GetDuration()
    {
        return 0.4f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {
            transform.localScale = Vector3.one * (1 + 2 * Mathf.Min(GetProgress(), 1 - GetProgress()));
        }
    }
}

public class FxFreesbyBalcony : FxTransform
{
    public FxFreesbyBalcony(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.3f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {
            float scale = (1 + 2 * Mathf.Min(GetProgress(), 1 - GetProgress()));
            transform.localScale = new Vector3(scale, 1, scale);
            transform.localRotation = local_rotation * Quaternion.AngleAxis(GetProgress() * 360, Vector3.up);
        }
    }
}

public class FxTreeCrush : FxTransform
{
    public FxTreeCrush(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.4f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {
            float scale = 2 * Mathf.Min(GetProgress(), 1 - GetProgress());
            transform.localScale = new Vector3(1 - 0.7f * scale, 1 + 5 * scale, 1 - 0.7f * scale);
            transform.localRotation = local_rotation * Quaternion.AngleAxis(scale * 90, local_rotation * Vector3.left);
        }
    }
}

public class FxShutterShaker : FxTransform
{
    public FxShutterShaker(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.8f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {
            float scale = 2 * Mathf.Min(GetProgress(), 1 - GetProgress());
            transform.localPosition = local_position + (local_rotation * Vector3.right) * (0.1f * scale);
            transform.localRotation = local_rotation * Quaternion.AngleAxis(Mathf.Cos(GetProgress() * Mathf.PI * 6) * 30, Vector3.forward);
        }
    }
}

public class FxChimChimney : FxTransform
{
    public FxChimChimney(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.3f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {
            float scale = 2 * Mathf.Min(GetProgress(), 1 - GetProgress());
            transform.localPosition = local_position + new Vector3(0, scale * 0.1f, 0);
        }
    }
}

// Factory
public class FxSoundTheAlarm : FxTransform
{
    public FxSoundTheAlarm(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.4f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {
        	float scale;
        	if (GetProgress() < 0.5f) {
				scale = 1.0f - GetProgress();
        	} else {
        		scale = GetProgress();
        	}
            transform.localScale = new Vector3(scale/2.0f, 1.0f, scale / 2.0f);
        }
    }
}
public class FxStaffEvacuation : FxTransform
{
    public FxStaffEvacuation(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.4f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {
            transform.localPosition = transform.localPosition + new Vector3(-GetProgress(), 0, 0);
        }
    }
}
public class FxPollutionPuke : FxTransform
{
    public FxPollutionPuke(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.4f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {
            transform.localRotation = local_rotation * Quaternion.AngleAxis(Mathf.Cos(GetProgress() * Mathf.PI * 6) * 30, Vector3.forward);
        }
    }
}

// Skyscraper
public class FxSpikySpiky : FxTransform
{
    public FxSpikySpiky(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.4f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {
            float scale;
            if (GetProgress() < 0.5f)
            {
                scale = 1.0f - GetProgress();
            }
            else
            {
                scale = GetProgress();
            }
            transform.localScale = new Vector3(scale, 1.0f, scale);
        }
    }
}
public class FxShurikenFromTheSky : FxTransform
{
    public FxShurikenFromTheSky(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.4f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {

        }
    }
}
public class FxSnakescraper : FxTransform
{
    public FxSnakescraper(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.4f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {

        }
    }
}

// Catedral
public class FxBellyBelly : FxTransform
{
    public FxBellyBelly(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.4f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {
            float scale;
            if (GetProgress() < 0.5f)
            {
                scale = 1.0f - GetProgress();
            }
            else
            {
                scale = GetProgress();
            }
            transform.localScale = new Vector3(scale/4.0f, 1.0f, scale / 4.0f);
        }
    }
}
public class FxSacrushedChapel : FxTransform
{
    public FxSacrushedChapel(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.4f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {
            float t;
            if (GetProgress() < 0.5f)
            {
                t = GetProgress();
            }
            else
            {
                t = 1.0f - GetProgress();
            }
            transform.localPosition = transform.localPosition + new Vector3(0, t*2, 0);
        }
    }
}
public class FxGrowingFaith : FxTransform
{
    public FxGrowingFaith(Transform mesh_transform) : base(mesh_transform)
    {
    }
    public override float GetDuration()
    {
        return 0.4f;
    }
    public override void Update()
    {
        base.Update();
        if (GetProgress() < 1)
        {
            transform.localScale = transform.localScale + new Vector3(GetProgress()/2.0f, GetProgress() / 2.0f, GetProgress() / 2.0f);
        }
    }
}