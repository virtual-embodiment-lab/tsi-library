//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Slides.v1;
//using Google.Apis.Slides.v1.Data;
//using Google.Apis.Services;
//using UnityEngine;
//using System.Linq;
//using System.Threading.Tasks;
//using UnityEngine.Networking;
//using System.Collections.Generic;
//using System.Collections;

//public class SlideshowManager : MonoBehaviour
//{
//    // Replace this with the path to your JSON credentials file
//    [SerializeField] string credentialsFile = "credentials.json";

//    // Replace this with the ID of your Google Slides presentation
//    [SerializeField] string presentationId = "YOUR_PRESENTATION_ID";

//    // The canvas image element to display the slideshow
//    [SerializeField] public UnityEngine.UI.Image image;

//    // The interval between slide changes (in seconds)
//    [SerializeField] public float interval = 3.0f;

//    // The list of images to display in the slideshow
//    [SerializeField] List<Texture2D> images;

//    // The current slide index
//    [SerializeField] int currentIndex;

//    void Start()
//    {
//        // Load the list of images from the Google Slides presentation
//        LoadImages().Wait();

//        // Start the slideshow
//        StartCoroutine(ShowSlides());
//    }

//    async Task LoadImages()
//    {
//        var service = new SlidesService(new BaseClientService.Initializer
//        {
//            HttpClientInitializer = GoogleCredential.FromAccessToken("AIzaSyAkpglb9B27sQ5dD_XLAlaG7YGSwB9EIiw").CreateScoped(SlidesService.Scope.Presentations),
//            ApplicationName = "My Slides App",
//            HttpClientFactory = new MyHttpClientFactory()
//        });

//        // Get the presentation
//        Presentation presentation = await service.Presentations.Get(presentationId).ExecuteAsync();

//        List<string> slideIds = (List<string>)presentation.Slides;

//        // Loop through each slide and download the JPEG image
//        images = new List<Texture2D>();
//        foreach (string slideId in slideIds)
//        {
//            // Get the slide
//            Page page = await service.Presentations.Pages.Get(presentationId, slideId).ExecuteAsync();

//            // Get the list of elements on the slide
//            List<PageElement> elements = page.PageElements.ToList();

//            // Find the first image on the slide
//            PageElement imageElement = elements.FirstOrDefault(e => e.Shape != null && e.Image != null);
//            if (imageElement != null)
//            {
//                // Get the image URL
//                string imageUrl = imageElement.Image.ContentUrl;

//                // Download the image
//                byte[] imageData = DownloadImage(imageUrl);

//                // Load the image into a Texture2D object
//                Texture2D texture = new Texture2D(2, 2);
//                texture.LoadImage(imageData);

//                // Add the image to the list
//                images.Add(texture);
//            }
//        }
//    }

//    IEnumerator ShowSlides()
//    {
//        while (true)
//        {
//            // Display the current slide
//            image.sprite = Sprite.Create(images[currentIndex], new Rect(0, 0, images[currentIndex].width, images[currentIndex].height), Vector2.zero);

//            // Wait for the interval time
//            yield return new WaitForSeconds(interval);

//            // Go to the next slide
//            currentIndex = (currentIndex + 1) % images.Count;
//        }
//    }

//    byte[] DownloadImage(string url)
//    {
//        UnityWebRequest request = UnityWebRequest.Get(url);
//        request.SendWebRequest();
//        new WaitUntil(() => request.isDone);
//        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
//        {
//            Debug.LogError(request.error);
//            return null;
//        }
//        return request.downloadHandler.data;
//    }
//}


using StandardLogging;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SlideshowManager : Tracker
{
    //public float slideShowInterval = 5f; // time in seconds between each slide
    public Sprite[] slides; // array of sprites to use as slides
    public Image slideImage; // reference to the Image UI component
    [SerializeField] bool replayMode = false;

    private int currentSlide = 0; // index of the current slide

    private void Start()
    {
        // set the sprite of the Image UI to the current slide
        slideImage.sprite = slides[currentSlide];
    }

    public override void StartReplayMode()
    {
        base.StartReplayMode();
        replayMode = true;
    }

    public override void ApplyValue(string type, string value)
    {
        base.ApplyValue(type, value);
        if (type == logtype.State.ToString())
        {
            int.TryParse(value, out currentSlide);
            slideImage.sprite = slides[currentSlide];
        }
    }

    protected override void Update()
    {
        if (replayMode)
        {
            return;
        }
        base.Update();

        // check if the "E" key is pressed
        if (InputSystem.GetDevice<Keyboard>().eKey.wasPressedThisFrame || OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Four))
        {
            // move to the next slide
            currentSlide = (currentSlide + 1) % slides.Length;
            slideImage.sprite = slides[currentSlide];
        }
        // check if the "Q" key is pressed
        else if (InputSystem.GetDevice<Keyboard>().qKey.wasPressedThisFrame || OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
        {
            // move to the previous slide
            currentSlide = (currentSlide - 1 + slides.Length) % slides.Length;
            slideImage.sprite = slides[currentSlide];
        }
        Map.UpdateOrCreate(new KVPair<logtype, string>(logtype.State, currentSlide.ToString()));
    }

}
