using System;
using Godot;

namespace ApophisSoftware;

public static class ImageManipulation {
	public static Texture2D AdjustBCS(Image Source, float Brightness, float Contrast, float Saturation) {
		Image proxy = new Image();
		proxy.CopyFrom(Source);

		proxy.AdjustBcs(Brightness, Contrast, Saturation);
		return ImageTexture.CreateFromImage(proxy);
	}

	public static Texture2D Clip(Image Source, float Percentage, ClipImgSpecs HowToClip) {
		// Get the size of the image
		int width = Source.GetWidth();
		int height = Source.GetHeight();

		Image proxy = new();
		proxy.CopyFrom(Source);

		Percentage = Mathf.Clamp(Percentage, 0f, 1f);

		int pixels = 0;

		switch (HowToClip) {
			case ClipImgSpecs.HorizontalFromLeft:
				pixels = Mathf.FloorToInt(width * Percentage);

				// Loop through each pixel and apply clip
				for (int x = 0; x < width; x++) {
					for (int y = 0; y < height; y++) {
						// Get the original pixel color

						if (x > pixels) {
							Color originalColor = proxy.GetPixel(x, y);
							// make transparent.
							Color shiftedColor = originalColor;
							shiftedColor.A = 0;

							// Set the new color to the pixel
							proxy.SetPixel(x, y, shiftedColor);
						}
					}
				}

				break;

			case ClipImgSpecs.HorizontalFromRight:
				pixels = Mathf.FloorToInt(width * Percentage);

				// Loop through each pixel and apply clip
				for (int x = width; x > 0; x--) {
					for (int y = 0; y < height; y++) {
						// Get the original pixel color

						if (x < pixels) {
							Color originalColor = proxy.GetPixel(x, y);
							// make transparent.
							Color shiftedColor = originalColor;
							shiftedColor.A = 0;

							// Set the new color to the pixel
							proxy.SetPixel(x, y, shiftedColor);
						}
					}
				}

				break;

			case ClipImgSpecs.VerticalFromBottom:
				pixels = Mathf.FloorToInt(height * Percentage);

				// Loop through each pixel and apply clip
				for (int y = height; y > 0; y--) {
					for (int x = 0; x < width; x++) {
						// Get the original pixel color

						if (y < pixels) {
							Color originalColor = proxy.GetPixel(x, y);
							// make transparent.
							Color shiftedColor = originalColor;
							shiftedColor.A = 0;

							// Set the new color to the pixel
							proxy.SetPixel(x, y, shiftedColor);
						}
					}
				}

				break;

			case ClipImgSpecs.VerticalFromTop:
				pixels = Mathf.FloorToInt(height * Percentage);

				// Loop through each pixel and apply clip
				for (int y = 0; y < height; y++) {
					for (int x = 0; x < width; x++) {
						// Get the original pixel color

						if (y > pixels) {
							Color originalColor = proxy.GetPixel(x, y);
							// make transparent.
							Color shiftedColor = originalColor;
							shiftedColor.A = 0;

							// Set the new color to the pixel
							proxy.SetPixel(x, y, shiftedColor);
						}
					}
				}

				break;

			default:
				break;
		}


		return ImageTexture.CreateFromImage(proxy);
	}

	public static Texture2D ColorizeTexture(Image Source, Color ColorToUse) {
		Image tex = new Image();
		tex.CopyFrom(Source);

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

	public static Texture2D CreateTextureAtlas(string[] ImagePaths) {
		// Load the six textures
		ImageTexture[] textures = LoadTextures(ImagePaths);
		if (textures == null) {
			Logging.Log("error", "Failed to make Atlas Texture from given texture files.");
			return new AtlasTexture();
		}

		// Calculate the size of the texture atlas based on individual texture sizes
		int atlasWidth = textures.Length * textures[0].GetWidth();
		int atlasHeight = textures[0].GetHeight();

		// Resize the atlas image
		Image atlasImage = Image.Create(atlasWidth, atlasHeight, false, Image.Format.Rgba8);

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

	public static Texture2D CropImage(Image Source, int Height, int Width) {
		Image tex = new Image();
		tex.CopyFrom(Source);
		Source.Crop(Width, Height);
		return ImageTexture.CreateFromImage(tex);
	}

	public static Texture2D LoadImageFromFile(string FilePath) {
		Image tex;

		// Load each texture
		try {
			tex = Image.LoadFromFile(FilePath);
		} catch (Exception error) {
			// Handle loading error, e.g., print a message
			Logging.Log("error", $"Error loading texture: {FilePath}.\nError message: {error.Message}");
			Image notex = Image.LoadFromFile("res://Sprites/MissingTexture.png");
			notex.Fill(Colors.Fuchsia);
			return ImageTexture.CreateFromImage(notex);
		}

		return ImageTexture.CreateFromImage(tex);
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

	public static Texture2D MakeTransparent(Image Source, Color SrcColor) {
		Image proxy = new Image();
		proxy.CopyFrom(Source);

		int height = proxy.GetHeight();
		int width = proxy.GetWidth();

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Color pixel = proxy.GetPixel(x, y);
				pixel.A = 0;
				if (pixel == SrcColor) {
					proxy.SetPixel(x, y, pixel);
				}
			}
		}

		return ImageTexture.CreateFromImage(proxy);
	}

	public static Texture2D ManipulateTexture(Image Source, bool FlipHorizontal, bool FlipVertical,
	                                          RotateImgSpec Rotation = RotateImgSpec.None) {
		Image tex = new Image();
		tex.CopyFrom(Source);

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

	public static Texture2D MaskImage(Image Source, Image Mask) {
		Image proxy = new Image();
		proxy.CopyFrom(Source);
		Image proxyMask = new Image();
		proxyMask.CopyFrom(Mask);

		int height = proxy.GetHeight();
		int width = proxy.GetWidth();

		proxyMask.Resize(width, height, Image.Interpolation.Nearest); // make sure that the mask is the same size.

		Color removed = new Color(0f, 0f, 0f, 0f);

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Color pixel = proxyMask.GetPixel(x, y);
				if (pixel.A == 0 || pixel == Colors.Black) {
					proxy.SetPixel(x, y, removed);
				}
			}
		}

		return ImageTexture.CreateFromImage(proxy);
	}

	public static Texture2D ReplaceColor(Image Source, Color SrcColor, Color NewColor) {
		Image proxy = new Image();
		proxy.CopyFrom(Source);

		int height = proxy.GetHeight();
		int width = proxy.GetWidth();

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Color pixel = proxy.GetPixel(x, y);
				if (pixel == SrcColor) {
					proxy.SetPixel(x, y, NewColor);
				}
			}
		}

		return ImageTexture.CreateFromImage(proxy);
	}

	public static Texture2D ShiftHsv(Image Source, float HueShift, float SaturationShift, float ValueShift) {
		// Make a proxy...
		Image proxy = new Image();
		proxy.CopyFrom(Source);

		// Get the size of the image
		int width = proxy.GetWidth();
		int height = proxy.GetHeight();

		// Loop through each pixel and apply HSV shift
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				// Get the original pixel color
				Color originalColor = proxy.GetPixel(x, y);

				// Convert the color to HSV
				originalColor.ToHsv(out float h, out float s, out float v);

				// Apply shifts
				h = (h + HueShift) % 1.0f;
				s = Mathf.Clamp(s + SaturationShift, 0.0f, 1.0f);
				v = Mathf.Clamp(v + ValueShift, 0.0f, 1.0f);

				// Convert back to RGB
				Color shiftedColor = Color.FromHsv(h, s, v);

				// Set the new color to the pixel
				proxy.SetPixel(x, y, shiftedColor);
			}
		}

		return ImageTexture.CreateFromImage(proxy);
	}

	public static Texture2D ShiftHue(Image Source, float HueShift) {
		Image proxy = new Image();
		proxy.CopyFrom(Source);

		// Get the size of the image
		int width = proxy.GetWidth();
		int height = proxy.GetHeight();

		// Loop through each pixel and apply hue shift
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				// Get the original pixel color
				Color originalColor = proxy.GetPixel(x, y);

				// Convert the color to HSL
				originalColor.ToHsv(out float h, out float s, out float v);

				// Shift the hue
				h = (h + HueShift) % 1.0f;

				// Convert back to RGB
				Color shiftedColor = Color.FromHsv(h, s, v);

				// Set the new color to the pixel
				proxy.SetPixel(x, y, shiftedColor);
			}
		}

		return ImageTexture.CreateFromImage(proxy);
	}
}

public enum RotateImgSpec {
	None      = 0,
	Right90   = 1,
	Left90    = 2,
	Rotate180 = 4,
}

public enum ClipImgSpecs {
	VerticalFromTop     = 1,
	VerticalFromBottom  = 2,
	HorizontalFromLeft  = 3,
	HorizontalFromRight = 4,
}