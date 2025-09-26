import FirstPackage.SecondClass;

class FirstClass {

    public static void main(String[] s) {
        SecondClass o = new SecondClass(34, 35);
        int i, j;
        for (i = 1; i <= 8; i++) {
            for (j = 1; j <= 8; j++) {
                o.setX(i);
                o.setY(j);
                System.out.print(o.multiply());
                System.out.print(" ");
            }
            System.out.println();
        }
    }
}
