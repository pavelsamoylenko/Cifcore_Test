using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace _App.Runtime.Services
{
    
    
    public class WebSpriteLoader : ISpriteLoader
    {
        private Sprite _defaultIcon;
        
        public WebSpriteLoader([Inject(Id = "DEFAULT_SPRITE", Optional = true)] Sprite defaultIcon = null)
        {
            _defaultIcon = defaultIcon;
        }
        
        /// <summary>
        /// Loads an image from a URL and converts it to a Sprite.
        /// </summary>
        /// <param name="url">The URL of the image.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        /// <returns>The downloaded Sprite.</returns>
        public async UniTask<Sprite> LoadSpriteAsync(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                using var request = UnityWebRequestTexture.GetTexture(url);
                await request.SendWebRequest().WithCancellation(cancellationToken);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to load icon from URL: {url}. Error: {request.error}");
                    return null;
                }

                var texture = DownloadHandlerTexture.GetContent(request);
                if (texture == null)
                {
                    Debug.LogError($"Failed to load icon from URL: {url}. Texture is null.");
                    return _defaultIcon;
                }
                var sprite = TextureToSprite(texture);

                return sprite;
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Icon loading was cancelled.");
                return null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unexpected error while loading icon from URL: {url}. Exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Converts a Texture2D to a Sprite.
        /// </summary>
        /// <param name="texture">The texture to convert.</param>
        /// <returns>The converted Sprite.</returns>
        private static Sprite TextureToSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            
        }
    }

    public interface ISpriteLoader
    {
        /// <summary>
        /// Loads an image from a URL and converts it to a Sprite.
        /// </summary>
        /// <param name="url">The URL of the image.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        /// <returns>The downloaded Sprite.</returns>
        UniTask<Sprite> LoadSpriteAsync(string url, CancellationToken cancellationToken = default);
    }
}