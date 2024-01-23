using System;
using Godot;

namespace ApophisSoftware;

public static class TextureAtlasCreator {
	// Specify the paths to your six image files
	internal static string[] ImagePaths = new string[] {
		"res://path/to/texture1.png",
		"res://path/to/texture2.png",
		"res://path/to/texture3.png",
		"res://path/to/texture4.png",
		"res://path/to/texture5.png",
		"res://path/to/texture6.png"
	};

	public static Texture2D CreateTextureAtlas() {
		// Load the six textures
		ImageTexture[] textures = LoadTextures(ImagePaths);
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

	private static ImageTexture[] LoadTextures(string[] Paths) {
		ImageTexture[] textures = new ImageTexture[Paths.Length];

		for (int i = 0; i < Paths.Length; i++) {
			textures[i] = new ImageTexture();
			Image tex;

			// Load each texture
			try {
				tex = Image.LoadFromFile(Paths[i]);
			} catch (Exception error) {
				// Handle loading error, e.g., print a message
				Logging.Log("error", $"Error loading texture: {Paths[i]}.\nError message: {error.Message}");
				return null;
			}

			// Assign the image to the texture.
			textures[i] = ImageTexture.CreateFromImage(tex);
		}

		return textures;
	}

	public static Texture2D ManipulateTexture(string FilePath, bool FlipHorizontal, bool FlipVertical,
	                                          RotateImgSpec Rotation = RotateImgSpec.None) {
		ImageTexture imageTexture = new ImageTexture();
		Image tex;

		// Load each texture
		try {
			tex = Image.LoadFromFile(FilePath);
		} catch (Exception error) {
			// Handle loading error, e.g., print a message
			Logging.Log("error", $"Error loading texture: {FilePath}.\nError message: {error.Message}");
			return null;
		}

		// Flip horizontally
		if (FlipHorizontal)
			tex.FlipX();

		// Flip vertically
		if (FlipVertical)
			tex.FlipY();

		// Rotate the image

		switch (Rotation) {
			case RotateImgSpec.Right90:
				tex.Rotate90(ClockDirection.Clockwise); // Rotate Right
				break;
			case RotateImgSpec.Left90:
				tex.Rotate90(ClockDirection.Counterclockwise); // Rotate left
				break;
			case RotateImgSpec.Rotate180:
				tex.Rotate180(); // Rotate 180 degrees
				break;
			default:
				break;
		}

		// return the modified image.
		return ImageTexture.CreateFromImage(tex);
	}

	public static Texture2D ManipulateTexture(Image ModTexture, bool FlipHorizontal, bool FlipVertical,
	                                          RotateImgSpec Rotation = RotateImgSpec.None) {
		// Flip horizontally
		if (FlipHorizontal)
			ModTexture.FlipX();

		// Flip vertically
		if (FlipVertical)
			ModTexture.FlipY();

		// Rotate the image

		switch (Rotation) {
			case RotateImgSpec.Right90:
				ModTexture.Rotate90(ClockDirection.Clockwise); // Rotate Right
				break;
			case RotateImgSpec.Left90:
				ModTexture.Rotate90(ClockDirection.Counterclockwise); // Rotate left
				break;
			case RotateImgSpec.Rotate180:
				ModTexture.Rotate180(); // Rotate 180 degrees
				break;
			default:
				break;
		}

		// return the modified image.
		return ImageTexture.CreateFromImage(ModTexture);
	}

	public static Texture2D ColorizeTexture(string FilePath, Color ColorToUse) {
		ImageTexture imageTexture = new ImageTexture();
		Image tex;

		// Load each texture
		try {
			tex = Image.LoadFromFile(FilePath);
		} catch (Exception error) {
			// Handle loading error, e.g., print a message
			Logging.Log("error", $"Error loading texture: {FilePath}.\nError message: {error.Message}");
			return null;
		}

		// Get the size of the image
		int width = tex.GetWidth();
		int height = tex.GetHeight();

		// Loop through each pixel and apply colorization
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				// Get the original pixel color
				Color originalColor = tex.GetPixel(x, y);

				// Apply colorization (multiply the original color by the desired color)
				Color newColor = originalColor * ColorToUse;

				// Set the new color to the pixel
				tex.SetPixel(x, y, newColor);
			}
		}

		// return the modified image.
		return ImageTexture.CreateFromImage(tex);
	}
}

public enum RotateImgSpec {
	None      = 0,
	Right90   = 1,
	Left90    = 2,
	Rotate180 = 4,
}