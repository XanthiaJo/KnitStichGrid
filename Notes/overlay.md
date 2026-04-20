# Overlay Notes (Image + Measurement)

These notes capture the intended approach for an image overlay, calibration (in/cm),
and a measurement overlay in KnitStichGrid.

## Current Context

- Preview grid is rendered as an `ItemsControl` inside a `ScrollViewer`.
- Overlay controls exist in the UI (`OverlayImagePath`, `IsOverlayVisible`,
  `OverlayOpacity`), but there is no overlay rendering layer yet.

## Coordinate Spaces (Decision)

- Grid space (pattern units): stitches/rows, used for grid-aligned overlays.
  - Rectangle stored as: `(xSt, yRow, wSt, hRow)`
- Screen space: WPF DIPs (treated as px for layout math).
- Image space: bitmap pixels, used for freeform calibration points.

## Physical Scale (Decision)

Introduce a single preview physical scale so inches/cm are meaningful in the UI.

- `PixelsPerInch` (DIPs per inch), default `96`.
- Gauge conversions:
  - `stitchesPerIn = StitchesPer4Inches / 4`
  - `rowsPerIn = RowsPer4Inches / 4`
- Cell sizes:
  - `CellWidthPx = PixelsPerInch / stitchesPerIn`
  - `CellHeightPx = PixelsPerInch / rowsPerIn`

Note: this replaces the current mapping where `CellWidthPx`/`CellHeightPx` are
 directly derived from the 4-inch gauge numbers.

## WPF Layering (Decision)

Render everything in one shared surface sized to the preview:

- Container `Width/Height` bound to `PreviewGridWidthPx` / `PreviewGridHeightPx`.
- Layer 1: `Image` (reference image), with `RenderTransform` = uniform `Scale` +
  `Translate`.
- Layer 2: grid `ItemsControl` (toggle cells).
- Layer 3: overlay `Canvas` for:
  - calibration line + endpoints
  - measurement rectangle + handles
  - dimension lines + labels

## ViewModel State (Expected)

Existing:

- `OverlayImagePath`, `IsOverlayVisible`, `OverlayOpacity`

Add:

- Image transform:
  - `ImageScale` (uniform)
  - `ImageOffsetX`, `ImageOffsetY`
- Calibration:
  - `CalibP1ImgX/Y`, `CalibP2ImgX/Y` (stored in image-space pixels)
  - `CalibLengthInput` (double)
  - `CalibUnit` (`in` or `cm`)
- Measurement rectangle (grid-aligned):
  - `RectXSt`, `RectYRow`, `RectWSt`, `RectHRow`

## Calibration (Freeform, in/cm Input)

User draws a freeform line on the image and enters known length in inches or cm.

- Measured in image pixels:
  - `LimgPx = distance(CalibP1Img, CalibP2Img)`
- Convert input to inches:
  - `Lin = (unit == cm) ? (value / 2.54) : value`
- Desired on-screen length:
  - `LscreenPx = Lin * PixelsPerInch`
- Apply uniform image scale:
  - `ImageScale = LscreenPx / LimgPx`
- Keep translation (`ImageOffsetX/Y`) unchanged so user can align the image to the
  grid by eye after calibration.

Important: when user clicks/drags calibration endpoints, convert mouse position
from screen->image coords by inverting the current image transform.

## Measurement Overlay (Rectangle + Labels)

Rectangle is grid-aligned, draggable/resizable (Thumb handles). Labels show:

- Horizontal:
  - stitches: `RectWSt`
  - inches: `WIn = RectWSt / stitchesPerIn`
  - cm: `WCm = WIn * 2.54`
- Vertical:
  - rows: `RectHRow`
  - inches: `HIn = RectHRow / rowsPerIn`
  - cm: `HCm = HIn * 2.54`

## Interaction Notes

- Image alignment:
  - Pan: drag to adjust `ImageOffsetX/Y`
  - Zoom: mouse wheel to adjust `ImageScale` (uniform)
- Overlay interaction:
  - Calibration endpoints: freeform on image, stored in image coords.
  - Measurement rectangle: stored in grid units; screen position derived from cell
    size.

## Future (Explicitly Deferred)

- Rotation support (image transform).
- Edge/feature detection (top/bottom/left/right-most points).
- Snap modes (grid snapping for calibration, optional).
- Persisting overlay state to disk.
