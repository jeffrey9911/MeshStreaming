using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class StreamContainer : MonoBehaviour
{
    IMeshManager iMeshManager;

    public List<Mesh> Meshes {get; private set;} = new List<Mesh>();
    //public List<Material> Materials {get; private set;} = new List<Material>();
    public List<Texture2D> Textures = new List<Texture2D>();

    public VideoPlayer VideoContainer {get; private set;}
    public RenderTexture VideoTexture {get; private set;}

    void Start()
    {
        if (!transform.TryGetComponent<IMeshManager>(out iMeshManager))
        {
            Debug.LogError("[IMeshStreamer - Container] No IMeshManager found");
        }
    }

    public void InitVideoTexture(string url)
    {
        VideoContainer = gameObject.AddComponent<VideoPlayer>();
        VideoTexture = new RenderTexture(2048, 2048, 24);

        VideoContainer.playOnAwake = false;
        VideoContainer.targetTexture = VideoTexture;
        VideoContainer.url = url;

        VideoContainer.prepareCompleted += VideoContainerPrepared;

        VideoContainer.Prepare();
    }

    void VideoContainerPrepared(VideoPlayer videoPlayer)
    {
        videoPlayer.Play();
        videoPlayer.Pause();
        videoPlayer.frame = 0;

        StartCoroutine(iMeshManager.streamHandler.VideoTextureOnReady());
    }

    void VideoContainerFrameReady(VideoPlayer videoPlayer)
    {
        iMeshManager.streamPlayer.AVControlledFramePlay();
    }

    public void LoadMesh(Mesh mesh)
    {
        Meshes.Add(mesh);
    }

    public void LoadMaterial(Material material)
    {
        //Materials.Add(material);
    }

    public void LoadTexture(Texture2D texture)
    {
        Textures.Add(texture);
    }

    public void Clear()
    {
        Meshes.Clear();
        //Materials.Clear();
        Textures.Clear();
        VideoContainer.Stop();
        VideoContainer.targetTexture.Release();
        Destroy(VideoContainer);
        Destroy(VideoTexture);
    }
}
