using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class OpenCV : MonoBehaviour
{

    [DllImport("OpenCVLink")]
    private static extern IntPtr getCamera();
    [DllImport("OpenCVLink")]
    private static extern void releaseCamera(IntPtr camera);
    [DllImport("OpenCVLink")]
    private static extern void getCameraTexture(IntPtr camera, IntPtr data, int width, int height);

    private IntPtr camera_;
    private Texture2D texture_;
    private Color32[] pixels_;
    private GCHandle pixels_handle_;
    private IntPtr pixels_ptr_;

    void Start()
    {
        camera_ = getCamera();
        texture_ = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        pixels_ = texture_.GetPixels32();
        pixels_handle_ = GCHandle.Alloc(pixels_, GCHandleType.Pinned);
        pixels_ptr_ = pixels_handle_.AddrOfPinnedObject();
        gameObject.GetComponent<MeshRenderer>().material.mainTexture = texture_;
    }

    void Update()
    {
        getCameraTexture(camera_, pixels_ptr_, texture_.width, texture_.height);
        texture_.SetPixels32(pixels_);
        texture_.Apply();
    }

    void OnApplicationQuit()
    {
        pixels_handle_.Free();
        releaseCamera(camera_);
    }

}
