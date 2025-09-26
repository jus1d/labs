public class Main {

    public static void main(String[] args) {
        Vector v1 = new Vector(3);
        v1.set(0, 1.0);
        v1.set(1, -2.5);
        v1.set(2, 4.0);

        Vector v2 = new Vector(3);
        v2.set(0, 0.5);
        v2.set(1, 3.0);
        v2.set(2, -1.5);

        System.out.println("v1 = " + v1);
        System.out.println("v2 = " + v2);

        System.out.println("min(v1) = " + v1.min());
        System.out.println("max(v1) = " + v1.max());

        System.out.printf("|v1| = %.3f%n", v1.euclideanNorm());

        v1.multiplyBy(2);
        System.out.println("v1 * 2 = " + v1);

        v1.sort();
        System.out.println("v1 sorted = " + v1);

        Vector sum = Vector.add(v1, v2);
        double dot = Vector.dot(v1, v2);
        System.out.println("v1 + v2 = " + sum);
        System.out.println("v1 · v2 = " + dot);
    }
}
