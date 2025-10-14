import java.util.Arrays;
import java.util.Objects;

public class LiveAlbum implements MusicCollection {

    private int[] songDurations;
    private String concertTitle;
    private int recordingYear;
    private int introOutroDuration;

    public LiveAlbum() {
        this.songDurations = new int[0];
        this.concertTitle = "Unknown Concert";
        this.recordingYear = 2000;
        this.introOutroDuration = 0;
    }

    public LiveAlbum(
        int[] songDurations,
        String concertTitle,
        int recordingYear,
        int introOutroDuration
    ) {
        if (songDurations == null || songDurations.length == 0) {
            throw new InvalidAlbumDataException(
                "Song durations array cannot be null or empty"
            );
        }
        if (concertTitle == null || concertTitle.trim().isEmpty()) {
            throw new InvalidAlbumDataException(
                "Concert title cannot be null or empty"
            );
        }
        if (recordingYear < 1900 || recordingYear > 2100) {
            throw new InvalidAlbumDataException(
                "Recording year must be between 1900 and 2100"
            );
        }
        if (introOutroDuration < 0) {
            throw new InvalidAlbumDataException(
                "Intro/outro duration cannot be negative"
            );
        }

        this.songDurations = Arrays.copyOf(songDurations, songDurations.length);
        this.concertTitle = concertTitle;
        this.recordingYear = recordingYear;
        this.introOutroDuration = introOutroDuration;
    }

    @Override
    public String getTitle() {
        return concertTitle;
    }

    @Override
    public void setTitle(String title) {
        if (title == null || title.trim().isEmpty()) {
            throw new InvalidAlbumDataException(
                "Title cannot be null or empty"
            );
        }
        this.concertTitle = title;
    }

    @Override
    public int getYear() {
        return recordingYear;
    }

    @Override
    public void setYear(int year) {
        if (year < 1900 || year > 2100) {
            throw new InvalidAlbumDataException(
                "Year must be between 1900 and 2100"
            );
        }
        this.recordingYear = year;
    }

    @Override
    public int[] getDurations() {
        return Arrays.copyOf(songDurations, songDurations.length);
    }

    @Override
    public int getDuration(int index) {
        if (index < 0 || index >= songDurations.length) {
            throw new InvalidAlbumDataException("Invalid song index: " + index);
        }
        return songDurations[index];
    }

    @Override
    public void setDuration(int index, int duration) {
        if (index < 0 || index >= songDurations.length) {
            throw new InvalidAlbumDataException("Invalid song index: " + index);
        }
        if (duration <= 0) {
            throw new InvalidAlbumDataException("Duration must be positive");
        }
        songDurations[index] = duration;
    }

    public int getIntroOutroDuration() {
        return introOutroDuration;
    }

    @Override
    public int calculateTotalDuration() throws InvalidDurationException {
        int total = 0;

        for (int duration : songDurations) {
            if (duration < 0) {
                throw new InvalidDurationException(
                    "Song duration cannot be negative"
                );
            }
            total += duration;
        }

        int netDuration = total - introOutroDuration;

        if (netDuration <= 0) {
            throw new InvalidDurationException(
                "Net concert duration is non-positive after removing intro/outro"
            );
        }

        return netDuration;
    }

    @Override
    public String toString() {
        return String.format(
            "LiveAlbum{title='%s', year=%d, songs=%d, intro/outro=%ds}",
            concertTitle,
            recordingYear,
            songDurations.length,
            introOutroDuration
        );
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        LiveAlbum liveAlbum = (LiveAlbum) o;
        return (
            recordingYear == liveAlbum.recordingYear &&
            introOutroDuration == liveAlbum.introOutroDuration &&
            Arrays.equals(songDurations, liveAlbum.songDurations) &&
            Objects.equals(concertTitle, liveAlbum.concertTitle)
        );
    }

    @Override
    public int hashCode() {
        int result = Objects.hash(
            concertTitle,
            recordingYear,
            introOutroDuration
        );
        result = 31 * result + Arrays.hashCode(songDurations);
        return result;
    }
}
