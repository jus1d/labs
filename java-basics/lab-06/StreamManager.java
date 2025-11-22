import java.io.*;
import java.util.Arrays;
import java.util.Comparator;
import java.util.Scanner;

public class StreamManager {

    private static SpaceMissionFactory factory =
        new PlanetExplorationMissionFactory();

    public static void setSpaceMissionFactory(SpaceMissionFactory newFactory) {
        factory = newFactory;
    }

    public static SpaceMission createInstance() {
        return factory.createInstance();
    }

    public static SpaceMission createInstance(
        int[] resourceData,
        String missionName,
        int costPerUnit
    ) {
        return factory.createInstance(resourceData, missionName, costPerUnit);
    }

    public static void sortSpaceMission(SpaceMission[] array) {
        Arrays.sort(array);
    }

    public static void sortSpaceMission(
        SpaceMission[] array,
        Comparator<SpaceMission> comp
    ) {
        Arrays.sort(array, comp);
    }

    public static SpaceMission unmodifiableSpaceMission(SpaceMission mission) {
        return new UnmodifiableSpaceMission(mission);
    }

    public static void outputSpaceMission(SpaceMission o, OutputStream out)
        throws IOException {
        o.output(out);
    }

    public static SpaceMission inputSpaceMission(InputStream in)
        throws IOException {
        DataInputStream dis = new DataInputStream(in);

        String missionName = dis.readUTF();
        int costPerUnit = dis.readInt();
        int size = dis.readInt();

        int[] resourceData = new int[size];
        for (int i = 0; i < size; i++) {
            resourceData[i] = dis.readInt();
        }

        return createInstance(resourceData, missionName, costPerUnit);
    }

    public static void writeSpaceMission(SpaceMission o, Writer out)
        throws IOException {
        o.write(out);
    }

    public static SpaceMission readSpaceMission(Reader in) throws IOException {
        StreamTokenizer tokenizer = new StreamTokenizer(in);

        tokenizer.resetSyntax();
        tokenizer.wordChars(33, 255);
        tokenizer.whitespaceChars(0, 32);

        tokenizer.nextToken();
        String missionName = tokenizer.sval;

        tokenizer.nextToken();
        int costPerUnit = Integer.parseInt(tokenizer.sval);

        tokenizer.nextToken();
        int size = Integer.parseInt(tokenizer.sval);

        int[] resourceData = new int[size];
        for (int i = 0; i < size; i++) {
            tokenizer.nextToken();
            resourceData[i] = Integer.parseInt(tokenizer.sval);
        }

        return createInstance(resourceData, missionName, costPerUnit);
    }

    public static void serializeSpaceMission(SpaceMission o, OutputStream out)
        throws IOException {
        ObjectOutputStream oos = new ObjectOutputStream(out);
        oos.writeObject(o);
        oos.flush();
    }

    public static SpaceMission deserializeSpaceMission(InputStream in)
        throws IOException, ClassNotFoundException {
        ObjectInputStream ois = new ObjectInputStream(in);
        return (SpaceMission) ois.readObject();
    }

    public static void writeFormatSpaceMission(SpaceMission o, Writer out)
        throws IOException {
        PrintWriter pw = new PrintWriter(out);

        pw.printf("%s %d %d", o.getMissionName(), o.getCostPerUnit(), o.size());

        for (int i = 0; i < o.size(); i++) {
            pw.printf(" %d", o.getArrayElement(i));
        }
        pw.println();
        pw.flush();
    }

    public static SpaceMission readFormatSpaceMission(Scanner in) {
        String missionName = in.next();
        int costPerUnit = in.nextInt();
        int size = in.nextInt();

        int[] resourceData = new int[size];
        for (int i = 0; i < size; i++) {
            resourceData[i] = in.nextInt();
        }

        return createInstance(resourceData, missionName, costPerUnit);
    }

    public static SpaceMission synchronizedSpaceMission(SpaceMission mission) {
        return new SynchronizedSpaceMission(mission);
    }
}
