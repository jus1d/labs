public interface MusicCollection {
    String getTitle();
    void setTitle(String title);
    int getYear();
    void setYear(int year);
    int[] getDurations();
    int getDuration(int index);
    void setDuration(int index, int duration);
    int calculateTotalDuration() throws InvalidDurationException;
}
