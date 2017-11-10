using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraShake : MonoBehaviour {

    private Camera thisCamera;
    public PostProcessingProfile ppProfile;

    void Start()
    {
        thisCamera = gameObject.GetComponent<Camera>();
    }

    public void Shake(float amplitude, float duration, float intensityChromaticAberration, float dampStartPercentage = 0.75f)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCamera(amplitude, duration,intensityChromaticAberration, dampStartPercentage));
    }

    private IEnumerator ShakeCamera(float amplitude, float duration,float intensityChromaticAberration, float dampStartPercentage)
    {
        //ensure percentage is in a valid range
        dampStartPercentage = Mathf.Clamp(dampStartPercentage, 0.0f, 1.0f);
        
        float elapsedTime = 0.0f;
        float damp = 1.0f;

        Vector3 cameraOrigin = thisCamera.transform.position;

        while (elapsedTime < duration)
        {
            //Parte Que faz a aberração cromática ficar louca;
            
            var chromAberration = ppProfile.chromaticAberration.settings;
            chromAberration.intensity = intensityChromaticAberration;
            ppProfile.chromaticAberration.settings = chromAberration;

            elapsedTime += Time.deltaTime;

            float percentComplete = elapsedTime / duration;

            if (percentComplete >= dampStartPercentage && percentComplete <= 1.0f)
            {
                damp = 1.0f - percentComplete;
            }

            Vector2 offsetValues = Random.insideUnitCircle;

            offsetValues *= amplitude * damp;

            thisCamera.transform.position = new Vector3(offsetValues.x, offsetValues.y, cameraOrigin.z);

            yield return null;
            
        }
        var chromaAberration = ppProfile.chromaticAberration.settings;
        chromaAberration.intensity = 0.8f;
        ppProfile.chromaticAberration.settings = chromaAberration;
        thisCamera.transform.position = cameraOrigin;
    }

}
