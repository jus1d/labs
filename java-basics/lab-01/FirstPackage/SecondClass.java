package FirstPackage;

public class SecondClass {

    private int x;
    private int y;

    public SecondClass(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public int getX() {
        return this.x;
    }

    public int getY() {
        return this.y;
    }

    public void setX(int x) {
        this.x = x;
    }

    public void setY(int y) {
        this.y = y;
    }

    public int sum() {
        return this.x + this.y;
    }

    public int multiply() {
        return this.x * this.y;
    }
}
