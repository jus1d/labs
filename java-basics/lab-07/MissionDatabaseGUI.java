import javax.swing.*;
import javax.swing.border.*;
import javax.swing.filechooser.FileNameExtensionFilter;
import java.awt.*;
import java.awt.event.*;
import java.io.*;
import java.util.*;
import java.util.List;

public class MissionDatabaseGUI extends JFrame {

    private List<SpaceMission> missions = new ArrayList<>();
    private JPanel missionsPanel;
    private JScrollPane scrollPane;
    private ButtonGroup lafButtonGroup;

    public MissionDatabaseGUI() {
        setTitle("База данных космических миссий");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(900, 700);
        setLocationRelativeTo(null);

        initComponents();
        initMenu();

        setVisible(true);
    }

    private void initComponents() {
        setLayout(new BorderLayout());

        // Main panel for missions with vertical layout
        missionsPanel = new JPanel();
        missionsPanel.setLayout(new BoxLayout(missionsPanel, BoxLayout.Y_AXIS));
        missionsPanel.setBorder(BorderFactory.createEmptyBorder(10, 10, 10, 10));

        // Scroll pane with both scrollbars
        scrollPane = new JScrollPane(missionsPanel);
        scrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
        scrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
        scrollPane.getVerticalScrollBar().setUnitIncrement(16);

        add(scrollPane, BorderLayout.CENTER);

        // LAF selection panel
        JPanel lafPanel = createLafPanel();
        add(lafPanel, BorderLayout.SOUTH);
    }

    private JPanel createLafPanel() {
        JPanel panel = new JPanel(new FlowLayout(FlowLayout.LEFT));
        panel.setBorder(BorderFactory.createTitledBorder("Внешний вид (Look and Feel)"));

        lafButtonGroup = new ButtonGroup();

        UIManager.LookAndFeelInfo[] lafs = UIManager.getInstalledLookAndFeels();
        String currentLaf = UIManager.getLookAndFeel().getClass().getName();

        for (UIManager.LookAndFeelInfo laf : lafs) {
            JRadioButton rb = new JRadioButton(laf.getName());
            rb.setActionCommand(laf.getClassName());
            rb.addActionListener(e -> changeLookAndFeel(e.getActionCommand()));

            if (laf.getClassName().equals(currentLaf)) {
                rb.setSelected(true);
            }

            lafButtonGroup.add(rb);
            panel.add(rb);
        }

        return panel;
    }

    private void changeLookAndFeel(String lafClassName) {
        try {
            UIManager.setLookAndFeel(lafClassName);
            SwingUtilities.updateComponentTreeUI(this);
            pack();
            setSize(900, 700);
        } catch (Exception e) {
            JOptionPane.showMessageDialog(this,
                "Ошибка смены внешнего вида: " + e.getMessage(),
                "Ошибка",
                JOptionPane.ERROR_MESSAGE);
        }
    }

    private void initMenu() {
        JMenuBar menuBar = new JMenuBar();

        // File menu
        JMenu fileMenu = new JMenu("Файл");

        JMenu loadMenu = new JMenu("Загрузить базу");

        JMenuItem loadBinary = new JMenuItem("Бинарный формат (DataStream)");
        loadBinary.addActionListener(e -> loadFromFile(FileFormat.BINARY));

        JMenuItem loadText = new JMenuItem("Текстовый формат (StreamTokenizer)");
        loadText.addActionListener(e -> loadFromFile(FileFormat.TEXT));

        JMenuItem loadSerialized = new JMenuItem("Сериализация (ObjectStream)");
        loadSerialized.addActionListener(e -> loadFromFile(FileFormat.SERIALIZED));

        JMenuItem loadFormatted = new JMenuItem("Форматированный текст (PrintWriter/Scanner)");
        loadFormatted.addActionListener(e -> loadFromFile(FileFormat.FORMATTED));

        loadMenu.add(loadBinary);
        loadMenu.add(loadText);
        loadMenu.add(loadSerialized);
        loadMenu.add(loadFormatted);

        JMenu saveMenu = new JMenu("Сохранить базу");

        JMenuItem saveBinary = new JMenuItem("Бинарный формат (DataStream)");
        saveBinary.addActionListener(e -> saveToFile(FileFormat.BINARY));

        JMenuItem saveText = new JMenuItem("Текстовый формат (StreamTokenizer)");
        saveText.addActionListener(e -> saveToFile(FileFormat.TEXT));

        JMenuItem saveSerialized = new JMenuItem("Сериализация (ObjectStream)");
        saveSerialized.addActionListener(e -> saveToFile(FileFormat.SERIALIZED));

        JMenuItem saveFormatted = new JMenuItem("Форматированный текст (PrintWriter/Scanner)");
        saveFormatted.addActionListener(e -> saveToFile(FileFormat.FORMATTED));

        saveMenu.add(saveBinary);
        saveMenu.add(saveText);
        saveMenu.add(saveSerialized);
        saveMenu.add(saveFormatted);

        JMenuItem autoFill = new JMenuItem("Автоматическое заполнение");
        autoFill.addActionListener(e -> autoFillDatabase());

        JMenuItem exitItem = new JMenuItem("Выход");
        exitItem.addActionListener(e -> System.exit(0));

        fileMenu.add(loadMenu);
        fileMenu.add(saveMenu);
        fileMenu.addSeparator();
        fileMenu.add(autoFill);
        fileMenu.addSeparator();
        fileMenu.add(exitItem);

        // Edit menu (bonus)
        JMenu editMenu = new JMenu("Редактирование");

        JMenuItem addMission = new JMenuItem("Добавить миссию");
        addMission.addActionListener(e -> showAddMissionDialog());

        JMenuItem removeMission = new JMenuItem("Удалить миссию по номеру");
        removeMission.addActionListener(e -> showRemoveMissionDialog());

        editMenu.add(addMission);
        editMenu.add(removeMission);

        // View menu for LAF
        JMenu viewMenu = new JMenu("Вид");
        JMenu lafMenu = new JMenu("Внешний вид");

        UIManager.LookAndFeelInfo[] lafs = UIManager.getInstalledLookAndFeels();
        for (UIManager.LookAndFeelInfo laf : lafs) {
            JMenuItem lafItem = new JMenuItem(laf.getName());
            lafItem.addActionListener(e -> changeLookAndFeel(laf.getClassName()));
            lafMenu.add(lafItem);
        }

        viewMenu.add(lafMenu);

        menuBar.add(fileMenu);
        menuBar.add(editMenu);
        menuBar.add(viewMenu);

        setJMenuBar(menuBar);
    }

    private enum FileFormat {
        BINARY, TEXT, SERIALIZED, FORMATTED
    }

    private void loadFromFile(FileFormat format) {
        JFileChooser fileChooser = new JFileChooser(".");

        switch (format) {
            case BINARY:
                fileChooser.setFileFilter(new FileNameExtensionFilter("Бинарные файлы (*.dat)", "dat"));
                break;
            case TEXT:
            case FORMATTED:
                fileChooser.setFileFilter(new FileNameExtensionFilter("Текстовые файлы (*.txt)", "txt"));
                break;
            case SERIALIZED:
                fileChooser.setFileFilter(new FileNameExtensionFilter("Сериализованные файлы (*.ser)", "ser"));
                break;
        }

        if (fileChooser.showOpenDialog(this) == JFileChooser.APPROVE_OPTION) {
            File file = fileChooser.getSelectedFile();
            loadMissionsFromFile(file, format);
        }
    }

    private void loadMissionsFromFile(File file, FileFormat format) {
        missions.clear();

        try {
            switch (format) {
                case BINARY:
                    loadBinaryFormat(file);
                    break;
                case TEXT:
                    loadTextFormat(file);
                    break;
                case SERIALIZED:
                    loadSerializedFormat(file);
                    break;
                case FORMATTED:
                    loadFormattedFormat(file);
                    break;
            }

            refreshMissionsPanel();
            JOptionPane.showMessageDialog(this,
                "Загружено миссий: " + missions.size(),
                "Успех",
                JOptionPane.INFORMATION_MESSAGE);

        } catch (Exception e) {
            JOptionPane.showMessageDialog(this,
                "Ошибка загрузки: " + e.getMessage(),
                "Ошибка",
                JOptionPane.ERROR_MESSAGE);
        }
    }

    private void loadBinaryFormat(File file) throws IOException {
        try (FileInputStream fis = new FileInputStream(file);
             BufferedInputStream bis = new BufferedInputStream(fis)) {

            while (bis.available() > 0) {
                SpaceMission mission = StreamManager.inputSpaceMission(bis);
                missions.add(mission);
            }
        }
    }

    private void loadTextFormat(File file) throws IOException {
        try (FileReader reader = new FileReader(file);
             BufferedReader br = new BufferedReader(reader)) {

            String line;
            while ((line = br.readLine()) != null) {
                if (!line.trim().isEmpty()) {
                    StringReader sr = new StringReader(line);
                    SpaceMission mission = StreamManager.readSpaceMission(sr);
                    missions.add(mission);
                }
            }
        }
    }

    private void loadSerializedFormat(File file) throws IOException, ClassNotFoundException {
        try (FileInputStream fis = new FileInputStream(file)) {
            while (fis.available() > 0) {
                SpaceMission mission = StreamManager.deserializeSpaceMission(fis);
                missions.add(mission);
            }
        }
    }

    private void loadFormattedFormat(File file) throws IOException {
        try (Scanner scanner = new Scanner(file)) {
            while (scanner.hasNext()) {
                SpaceMission mission = StreamManager.readFormatSpaceMission(scanner);
                missions.add(mission);
            }
        }
    }

    private void saveToFile(FileFormat format) {
        if (missions.isEmpty()) {
            JOptionPane.showMessageDialog(this,
                "База данных пуста. Нечего сохранять.",
                "Информация",
                JOptionPane.INFORMATION_MESSAGE);
            return;
        }

        JFileChooser fileChooser = new JFileChooser(".");

        String defaultExt;
        switch (format) {
            case BINARY:
                fileChooser.setFileFilter(new FileNameExtensionFilter("Бинарные файлы (*.dat)", "dat"));
                defaultExt = ".dat";
                break;
            case TEXT:
            case FORMATTED:
                fileChooser.setFileFilter(new FileNameExtensionFilter("Текстовые файлы (*.txt)", "txt"));
                defaultExt = ".txt";
                break;
            case SERIALIZED:
                fileChooser.setFileFilter(new FileNameExtensionFilter("Сериализованные файлы (*.ser)", "ser"));
                defaultExt = ".ser";
                break;
            default:
                defaultExt = "";
        }

        if (fileChooser.showSaveDialog(this) == JFileChooser.APPROVE_OPTION) {
            File file = fileChooser.getSelectedFile();

            // Add extension if not present
            if (!file.getName().contains(".")) {
                file = new File(file.getAbsolutePath() + defaultExt);
            }

            saveMissionsToFile(file, format);
        }
    }

    private void saveMissionsToFile(File file, FileFormat format) {
        try {
            switch (format) {
                case BINARY:
                    saveBinaryFormat(file);
                    break;
                case TEXT:
                    saveTextFormat(file);
                    break;
                case SERIALIZED:
                    saveSerializedFormat(file);
                    break;
                case FORMATTED:
                    saveFormattedFormat(file);
                    break;
            }

            JOptionPane.showMessageDialog(this,
                "Сохранено миссий: " + missions.size() + "\nФайл: " + file.getName(),
                "Успех",
                JOptionPane.INFORMATION_MESSAGE);

        } catch (Exception e) {
            JOptionPane.showMessageDialog(this,
                "Ошибка сохранения: " + e.getMessage(),
                "Ошибка",
                JOptionPane.ERROR_MESSAGE);
        }
    }

    private void saveBinaryFormat(File file) throws IOException {
        try (FileOutputStream fos = new FileOutputStream(file);
             BufferedOutputStream bos = new BufferedOutputStream(fos)) {

            for (SpaceMission mission : missions) {
                StreamManager.outputSpaceMission(mission, bos);
            }
        }
    }

    private void saveTextFormat(File file) throws IOException {
        try (FileWriter writer = new FileWriter(file);
             BufferedWriter bw = new BufferedWriter(writer)) {

            for (SpaceMission mission : missions) {
                StreamManager.writeSpaceMission(mission, bw);
            }
        }
    }

    private void saveSerializedFormat(File file) throws IOException {
        try (FileOutputStream fos = new FileOutputStream(file)) {
            for (SpaceMission mission : missions) {
                StreamManager.serializeSpaceMission(mission, fos);
            }
        }
    }

    private void saveFormattedFormat(File file) throws IOException {
        try (FileWriter writer = new FileWriter(file);
             PrintWriter pw = new PrintWriter(writer)) {

            for (SpaceMission mission : missions) {
                StreamManager.writeFormatSpaceMission(mission, pw);
            }
        }
    }

    private void autoFillDatabase() {
        missions.clear();

        Random random = new Random();
        String[] planetNames = {"Альфа", "Бета", "Гамма", "Дельта", "Эпсилон", "Омега", "Сигма", "Тета"};
        String[] asteroidNames = {"Церера", "Паллада", "Веста", "Юнона", "Эрос", "Ида", "Матильда", "Ганимед"};

        // Generate Planet Exploration Missions
        StreamManager.setSpaceMissionFactory(new PlanetExplorationMissionFactory());
        for (int i = 0; i < 5; i++) {
            int size = random.nextInt(5) + 2;
            int[] resources = new int[size];
            for (int j = 0; j < size; j++) {
                resources[j] = random.nextInt(500) + 100;
            }
            String name = planetNames[random.nextInt(planetNames.length)] + "-" + (i + 1);
            int cost = random.nextInt(50) + 10;

            missions.add(StreamManager.createInstance(resources, name, cost));
        }

        // Generate Asteroid Mining Missions
        StreamManager.setSpaceMissionFactory(new AsteroidMiningMissionFactory());
        for (int i = 0; i < 5; i++) {
            int size = random.nextInt(5) + 2;
            int[] minerals = new int[size];
            for (int j = 0; j < size; j++) {
                minerals[j] = random.nextInt(300) + 50;
            }
            String name = asteroidNames[random.nextInt(asteroidNames.length)] + "-" + (i + 1);
            int cost = random.nextInt(30) + 5;

            missions.add(StreamManager.createInstance(minerals, name, cost));
        }

        refreshMissionsPanel();
        JOptionPane.showMessageDialog(this,
            "Автоматически создано миссий: " + missions.size(),
            "Успех",
            JOptionPane.INFORMATION_MESSAGE);
    }

    private void refreshMissionsPanel() {
        missionsPanel.removeAll();

        for (int i = 0; i < missions.size(); i++) {
            JPanel missionCard = createMissionCard(missions.get(i), i);
            missionsPanel.add(missionCard);
            missionsPanel.add(Box.createRigidArea(new Dimension(0, 10)));
        }

        missionsPanel.revalidate();
        missionsPanel.repaint();
    }

    private JPanel createMissionCard(SpaceMission mission, int index) {
        JPanel card = new JPanel();
        card.setLayout(new BorderLayout(10, 5));
        card.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(Color.GRAY, 1),
            BorderFactory.createEmptyBorder(10, 10, 10, 10)
        ));
        card.setMaximumSize(new Dimension(Integer.MAX_VALUE, 120));
        card.setBackground(new Color(245, 245, 250));
        card.setCursor(new Cursor(Cursor.HAND_CURSOR));

        // Header with index and name
        JPanel headerPanel = new JPanel(new FlowLayout(FlowLayout.LEFT));
        headerPanel.setOpaque(false);

        JLabel indexLabel = new JLabel("[" + (index + 1) + "]");
        indexLabel.setFont(new Font("Monospaced", Font.BOLD, 14));
        indexLabel.setForeground(Color.BLUE);

        String missionType = mission instanceof PlanetExplorationMission
            ? "Исследование планет"
            : "Добыча астероидов";
        JLabel typeLabel = new JLabel(missionType);
        typeLabel.setFont(new Font("Dialog", Font.ITALIC, 12));
        typeLabel.setForeground(Color.DARK_GRAY);

        JLabel nameLabel = new JLabel(mission.getMissionName());
        nameLabel.setFont(new Font("Dialog", Font.BOLD, 16));

        headerPanel.add(indexLabel);
        headerPanel.add(Box.createRigidArea(new Dimension(10, 0)));
        headerPanel.add(nameLabel);
        headerPanel.add(Box.createRigidArea(new Dimension(20, 0)));
        headerPanel.add(typeLabel);

        // Info panel
        JPanel infoPanel = new JPanel(new GridLayout(2, 1, 5, 5));
        infoPanel.setOpaque(false);

        JLabel costLabel = new JLabel("Стоимость за единицу: " + mission.getCostPerUnit());
        JLabel dataLabel = new JLabel("Данные: " + Arrays.toString(mission.getResourceData()));
        dataLabel.setFont(new Font("Monospaced", Font.PLAIN, 12));

        infoPanel.add(costLabel);
        infoPanel.add(dataLabel);

        card.add(headerPanel, BorderLayout.NORTH);
        card.add(infoPanel, BorderLayout.CENTER);

        // Click listener
        card.addMouseListener(new MouseAdapter() {
            @Override
            public void mouseClicked(MouseEvent e) {
                showMissionDetailsDialog(mission, index);
            }

            @Override
            public void mouseEntered(MouseEvent e) {
                card.setBackground(new Color(230, 230, 240));
            }

            @Override
            public void mouseExited(MouseEvent e) {
                card.setBackground(new Color(245, 245, 250));
            }
        });

        return card;
    }

    private void showMissionDetailsDialog(SpaceMission mission, int index) {
        JDialog dialog = new JDialog(this, "Детали миссии #" + (index + 1), true);
        dialog.setSize(500, 400);
        dialog.setLocationRelativeTo(this);
        dialog.setLayout(new BorderLayout(10, 10));

        JPanel contentPanel = new JPanel();
        contentPanel.setLayout(new BoxLayout(contentPanel, BoxLayout.Y_AXIS));
        contentPanel.setBorder(BorderFactory.createEmptyBorder(15, 15, 15, 15));

        // Mission index
        JLabel indexLabel = new JLabel("Номер объекта в базе: " + (index + 1));
        indexLabel.setFont(new Font("Dialog", Font.BOLD, 16));
        indexLabel.setAlignmentX(Component.LEFT_ALIGNMENT);

        // Mission type
        String missionType = mission instanceof PlanetExplorationMission
            ? "Миссия исследования планет"
            : "Миссия добычи астероидов";
        JLabel typeLabel = new JLabel("Тип: " + missionType);
        typeLabel.setAlignmentX(Component.LEFT_ALIGNMENT);

        // Mission name
        JLabel nameLabel = new JLabel("Название: " + mission.getMissionName());
        nameLabel.setAlignmentX(Component.LEFT_ALIGNMENT);

        // Cost
        JLabel costLabel = new JLabel("Стоимость за единицу: " + mission.getCostPerUnit());
        costLabel.setAlignmentX(Component.LEFT_ALIGNMENT);

        // Size
        JLabel sizeLabel = new JLabel("Количество элементов: " + mission.size());
        sizeLabel.setAlignmentX(Component.LEFT_ALIGNMENT);

        // Resource data
        JLabel dataLabel = new JLabel("Данные ресурсов: " + Arrays.toString(mission.getResourceData()));
        dataLabel.setAlignmentX(Component.LEFT_ALIGNMENT);

        // Business method result
        JLabel profitLabel;
        try {
            int profit = mission.calculateNetProfit();
            profitLabel = new JLabel("Результат бизнес-метода (чистая прибыль): " + profit);
            profitLabel.setForeground(new Color(0, 128, 0));
        } catch (MissionBusinessException e) {
            profitLabel = new JLabel("Бизнес-метод: " + e.getMessage());
            profitLabel.setForeground(Color.RED);
        }
        profitLabel.setFont(new Font("Dialog", Font.BOLD, 14));
        profitLabel.setAlignmentX(Component.LEFT_ALIGNMENT);

        // Full toString
        JTextArea toStringArea = new JTextArea(mission.toString());
        toStringArea.setEditable(false);
        toStringArea.setLineWrap(true);
        toStringArea.setWrapStyleWord(true);
        toStringArea.setBackground(new Color(240, 240, 240));
        toStringArea.setBorder(BorderFactory.createTitledBorder("Полное описание (toString)"));
        toStringArea.setAlignmentX(Component.LEFT_ALIGNMENT);

        contentPanel.add(indexLabel);
        contentPanel.add(Box.createRigidArea(new Dimension(0, 10)));
        contentPanel.add(typeLabel);
        contentPanel.add(Box.createRigidArea(new Dimension(0, 5)));
        contentPanel.add(nameLabel);
        contentPanel.add(Box.createRigidArea(new Dimension(0, 5)));
        contentPanel.add(costLabel);
        contentPanel.add(Box.createRigidArea(new Dimension(0, 5)));
        contentPanel.add(sizeLabel);
        contentPanel.add(Box.createRigidArea(new Dimension(0, 5)));
        contentPanel.add(dataLabel);
        contentPanel.add(Box.createRigidArea(new Dimension(0, 15)));
        contentPanel.add(profitLabel);
        contentPanel.add(Box.createRigidArea(new Dimension(0, 15)));
        contentPanel.add(toStringArea);

        JButton closeButton = new JButton("Закрыть");
        closeButton.addActionListener(e -> dialog.dispose());

        JPanel buttonPanel = new JPanel(new FlowLayout(FlowLayout.RIGHT));
        buttonPanel.add(closeButton);

        dialog.add(new JScrollPane(contentPanel), BorderLayout.CENTER);
        dialog.add(buttonPanel, BorderLayout.SOUTH);

        dialog.setVisible(true);
    }

    private void showAddMissionDialog() {
        JDialog dialog = new JDialog(this, "Добавить миссию", true);
        dialog.setSize(450, 350);
        dialog.setLocationRelativeTo(this);
        dialog.setLayout(new BorderLayout(10, 10));

        JPanel formPanel = new JPanel(new GridBagLayout());
        formPanel.setBorder(BorderFactory.createEmptyBorder(15, 15, 15, 15));
        GridBagConstraints gbc = new GridBagConstraints();
        gbc.insets = new Insets(5, 5, 5, 5);
        gbc.anchor = GridBagConstraints.WEST;

        // Mission type
        gbc.gridx = 0; gbc.gridy = 0;
        formPanel.add(new JLabel("Тип миссии:"), gbc);

        JComboBox<String> typeCombo = new JComboBox<>(new String[]{
            "Исследование планет", "Добыча астероидов"
        });
        gbc.gridx = 1; gbc.fill = GridBagConstraints.HORIZONTAL;
        formPanel.add(typeCombo, gbc);

        // Mission name
        gbc.gridx = 0; gbc.gridy = 1; gbc.fill = GridBagConstraints.NONE;
        formPanel.add(new JLabel("Название:"), gbc);

        JTextField nameField = new JTextField(20);
        gbc.gridx = 1; gbc.fill = GridBagConstraints.HORIZONTAL;
        formPanel.add(nameField, gbc);

        // Cost per unit
        gbc.gridx = 0; gbc.gridy = 2; gbc.fill = GridBagConstraints.NONE;
        formPanel.add(new JLabel("Стоимость за единицу:"), gbc);

        JSpinner costSpinner = new JSpinner(new SpinnerNumberModel(10, 0, 10000, 1));
        gbc.gridx = 1; gbc.fill = GridBagConstraints.HORIZONTAL;
        formPanel.add(costSpinner, gbc);

        // Resource data
        gbc.gridx = 0; gbc.gridy = 3; gbc.fill = GridBagConstraints.NONE;
        formPanel.add(new JLabel("Данные (через запятую):"), gbc);

        JTextField dataField = new JTextField(20);
        dataField.setToolTipText("Введите числа через запятую, например: 100, 200, 300");
        gbc.gridx = 1; gbc.fill = GridBagConstraints.HORIZONTAL;
        formPanel.add(dataField, gbc);

        // Buttons
        JPanel buttonPanel = new JPanel(new FlowLayout(FlowLayout.RIGHT));
        JButton addButton = new JButton("Добавить");
        JButton cancelButton = new JButton("Отмена");

        addButton.addActionListener(e -> {
            try {
                String name = nameField.getText().trim();
                if (name.isEmpty()) {
                    throw new IllegalArgumentException("Введите название миссии");
                }

                int cost = (Integer) costSpinner.getValue();

                String[] dataStrings = dataField.getText().split(",");
                if (dataStrings.length == 0 || dataField.getText().trim().isEmpty()) {
                    throw new IllegalArgumentException("Введите данные ресурсов");
                }

                int[] data = new int[dataStrings.length];
                for (int i = 0; i < dataStrings.length; i++) {
                    data[i] = Integer.parseInt(dataStrings[i].trim());
                }

                if (typeCombo.getSelectedIndex() == 0) {
                    StreamManager.setSpaceMissionFactory(new PlanetExplorationMissionFactory());
                } else {
                    StreamManager.setSpaceMissionFactory(new AsteroidMiningMissionFactory());
                }

                SpaceMission mission = StreamManager.createInstance(data, name, cost);
                missions.add(mission);
                refreshMissionsPanel();
                dialog.dispose();

                JOptionPane.showMessageDialog(this, "Миссия добавлена успешно!");

            } catch (NumberFormatException ex) {
                JOptionPane.showMessageDialog(dialog,
                    "Неверный формат данных. Введите целые числа через запятую.",
                    "Ошибка",
                    JOptionPane.ERROR_MESSAGE);
            } catch (Exception ex) {
                JOptionPane.showMessageDialog(dialog,
                    "Ошибка: " + ex.getMessage(),
                    "Ошибка",
                    JOptionPane.ERROR_MESSAGE);
            }
        });

        cancelButton.addActionListener(e -> dialog.dispose());

        buttonPanel.add(addButton);
        buttonPanel.add(cancelButton);

        dialog.add(formPanel, BorderLayout.CENTER);
        dialog.add(buttonPanel, BorderLayout.SOUTH);

        dialog.setVisible(true);
    }

    private void showRemoveMissionDialog() {
        if (missions.isEmpty()) {
            JOptionPane.showMessageDialog(this,
                "База данных пуста",
                "Информация",
                JOptionPane.INFORMATION_MESSAGE);
            return;
        }

        String input = JOptionPane.showInputDialog(this,
            "Введите номер миссии для удаления (1-" + missions.size() + "):",
            "Удалить миссию",
            JOptionPane.QUESTION_MESSAGE);

        if (input != null && !input.trim().isEmpty()) {
            try {
                int index = Integer.parseInt(input.trim()) - 1;
                if (index < 0 || index >= missions.size()) {
                    throw new IndexOutOfBoundsException();
                }

                SpaceMission removed = missions.remove(index);
                refreshMissionsPanel();

                JOptionPane.showMessageDialog(this,
                    "Удалена миссия: " + removed.getMissionName(),
                    "Успех",
                    JOptionPane.INFORMATION_MESSAGE);

            } catch (NumberFormatException e) {
                JOptionPane.showMessageDialog(this,
                    "Введите корректное число",
                    "Ошибка",
                    JOptionPane.ERROR_MESSAGE);
            } catch (IndexOutOfBoundsException e) {
                JOptionPane.showMessageDialog(this,
                    "Неверный номер миссии. Допустимые значения: 1-" + missions.size(),
                    "Ошибка",
                    JOptionPane.ERROR_MESSAGE);
            }
        }
    }

    public static void main(String[] args) {
        try {
            UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
        } catch (Exception e) {
            // Use default LAF
        }

        SwingUtilities.invokeLater(() -> new MissionDatabaseGUI());
    }
}
