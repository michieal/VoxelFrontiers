using System;
using Godot;

namespace ApophisSoftware;

public static class TextureAtlasCreator {
	// Specify the paths to your six image files
	internal static string[] imagePaths = new string[] {
		"res://path/to/texture1.png",
		"res://path/to/texture2.png",
		"res://path/to/texture3.png",
		"res://path/to/texture4.png",
		"res://path/to/texture5.png",
		"res://path/to/texture6.png"
	};

	public static Texture2D CreateTextureAtlas() {
		// Load the six textures
		ImageTexture[] textures = LoadTextures(imagePaths);
		if (textures == null) {
			Logging.Log("error", "Failed to make Atlas Texture from given texture files.");
			return new AtlasTexture();
		}

		// Create a new image for the texture atlas
		Image atlasImage = new Image();

		// Calculate the size of the texture atlas based on individual texture sizes
		int atlasWidth = textures.Length * textures[0].GetWidth();
		int atlasHeight = textures[0].GetHeight();

		// Resize the atlas image
		atlasImage = Image.Create(atlasWidth, atlasHeight, false, Image.Format.Rgba8);

		// Copy individual textures to the atlas
		for (int i = 0; i < textures.Length; i++) {
			//TODO: Add in code for knowing which image is what. 
			ImageTexture texture = textures[i];
			Image textureImage = texture.GetImage();
			atlasImage.BlitRect(textureImage,
				new Rect2I(i * texture.GetWidth(), 0, texture.GetWidth(), texture.GetHeight()), Vector2I.Zero);
		}

		// Create and Return a new texture for the atlas
		return ImageTexture.CreateFromImage(atlasImage);
	}

	private static ImageTexture[] LoadTextures(string[] paths) {
		ImageTexture[] textures = new ImageTexture[paths.Length];

		for (int i = 0; i < paths.Length; i++) {
			textures[i] = new ImageTexture();
			Image tex;

			// Load each texture
			try {
				tex = Image.LoadFromFile(paths[i]);
			} catch (Exception error) {
				// Handle loading error, e.g., print a message
				Logging.Log("error", $"Error loading texture: {paths[i]}.\nError message: {error.Message}");
				return null;
			}

			// Assign the image to the texture.
			textures[i] = ImageTexture.CreateFromImage(tex);
		}

		return textures;
	}
}