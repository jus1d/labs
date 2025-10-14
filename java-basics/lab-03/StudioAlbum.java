import java.util.Arrays;
import java.util.Objects;

public class StudioAlbum implements MusicCollection {

    private int[] trackDurations;
    private String albumTitle;
    private int releaseYear;
    private int minTrackDuration;

    public StudioAlbum() {
        this.trackDurations = new int[0];
        this.albumTitle = "Unknown Album";
        this.releaseYear = 2000;
        this.minTrackDuration = 30;
    }

    public StudioAlbum(
        int[] trackDurations,
        String albumTitle,
        int releaseYear,
        int minTrackDuration
    ) {
        if (trackDurations == null || trackDurations.length == 0) {
            throw new InvalidAlbumDataException(
                "Track durations array cannot be null or empty"
            );
        }
        if (albumTitle == null || albumTitle.trim().isEmpty()) {
            throw new InvalidAlbumDataException(
                "Album title cannot be null or empty"
            );
        }
        if (releaseYear < 1900 || releaseYear > 2100) {
            throw new InvalidAlbumDataException(
                "Release year must be between 1900 and 2100"
            );
        }
        if (minTrackDuration <= 0) {
            throw new InvalidAlbumDataException(
                "Minimum track duration must be positive"
            );
        }

        this.trackDurations = Arrays.copyOf(
            trackDurations,
            trackDurations.length
        );
        this.albumTitle = albumTitle;
        this.releaseYear = releaseYear;
        this.minTrackDuration = minTrackDuration;
    }

    @Override
    public String getTitle() {
        return albumTitle;
    }

    @Override
    public void setTitle(String title) {
        if (title == null || title.trim().isEmpty()) {
            throw new InvalidAlbumDataException(
                "Title cannot be null or empty"
            );
        }
        this.albumTitle = title;
    }

    @Override
    public int getYear() {
        return releaseYear;
    }

    @Override
    public void setYear(int year) {
        if (year < 1900 || year > 2100) {
            throw new InvalidAlbumDataException(
                "Year must be between 1900 and 2100"
            );
        }
        this.releaseYear = year;
    }

    @Override
    public int[] getDurations() {
        return Arrays.copyOf(trackDurations, trackDurations.length);
    }

    @Override
    public int getDuration(int index) {
        if (index < 0 || index >= trackDurations.length) {
            throw new InvalidAlbumDataException(
                "Invalid track index: " + index
            );
        }
        return trackDurations[index];
    }

    @Override
    public void setDuration(int index, int duration) {
        if (index < 0 || index >= trackDurations.length) {
            throw new InvalidAlbumDataException(
                "Invalid track index: " + index
            );
        }
        if (duration <= 0) {
            throw new InvalidAlbumDataException("Duration must be positive");
        }
        trackDurations[index] = duration;
    }

    public int getMinTrackDuration() {
        return minTrackDuration;
    }

    @Override
    public int calculateTotalDuration() throws InvalidDurationException {
        int total = 0;
        boolean hasValidTrack = false;

        for (int duration : trackDurations) {
            if (duration < 0) {
                throw new InvalidDurationException(
                    "Track duration cannot be negative"
                );
            }
            if (duration >= minTrackDuration) {
                total += duration;
                hasValidTrack = true;
            }
        }

        if (!hasValidTrack) {
            throw new InvalidDurationException(
                "No valid tracks found (all tracks shorter than " +
                    minTrackDuration +
                    " seconds)"
            );
        }

        return total;
    }

    @Override
    public String toString() {
        return String.format(
            "StudioAlbum{title='%s', year=%d, tracks=%d, minDuration=%ds}",
            albumTitle,
            releaseYear,
            trackDurations.length,
            minTrackDuration
        );
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        StudioAlbum that = (StudioAlbum) o;
        return (
            releaseYear == that.releaseYear &&
            minTrackDuration == that.minTrackDuration &&
            Arrays.equals(trackDurations, that.trackDurations) &&
            Objects.equals(albumTitle, that.albumTitle)
        );
    }

    @Override
    public int hashCode() {
        int result = Objects.hash(albumTitle, releaseYear, minTrackDuration);
        result = 31 * result + Arrays.hashCode(trackDurations);
        return result;
    }
}
