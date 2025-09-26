import java.util.Arrays;

public class Vector {

    private final double[] coords;

    public Vector(int length) {
        if (length <= 0) throw new IllegalArgumentException(
            "Length must be greater than zero"
        );
        this.coords = new double[length];
    }

    public double get(int index) {
        checkIndex(index);
        return coords[index];
    }

    public void set(int index, double value) {
        checkIndex(index);
        coords[index] = value;
    }

    public int length() {
        return coords.length;
    }

    public double min() {
        ensureNotEmpty();
        double m = coords[0];
        for (int i = 1; i < coords.length; i++) if (coords[i] < m) m =
            coords[i];
        return m;
    }

    public double max() {
        ensureNotEmpty();
        double m = coords[0];
        for (int i = 1; i < coords.length; i++) if (coords[i] > m) m =
            coords[i];
        return m;
    }

    public void sort() {
        Arrays.sort(coords);
    }

    public double euclideanNorm() {
        double sum = 0;
        for (double v : coords) sum += v * v;
        return Math.sqrt(sum);
    }

    public void multiplyBy(double factor) {
        for (int i = 0; i < coords.length; i++) coords[i] *= factor;
    }

    public static Vector add(Vector a, Vector b) {
        checkSameLength(a, b);
        Vector res = new Vector(a.length());
        for (int i = 0; i < a.length(); i++) res.coords[i] =
            a.coords[i] + b.coords[i];
        return res;
    }

    public static double dot(Vector a, Vector b) {
        checkSameLength(a, b);
        double sum = 0;
        for (int i = 0; i < a.length(); i++) sum += a.coords[i] * b.coords[i];
        return sum;
    }

    private void checkIndex(int i) {
        if (i < 0 || i >= coords.length) throw new IndexOutOfBoundsException(
            "Index " + i
        );
    }

    private void ensureNotEmpty() {
        if (coords.length == 0) throw new IllegalStateException(
            "Vector is empty"
        );
    }

    private static void checkSameLength(Vector a, Vector b) {
        if (a.length() != b.length()) throw new IllegalArgumentException(
            "Different lengths"
        );
    }

    @Override
    public String toString() {
        return Arrays.toString(coords);
    }
}
