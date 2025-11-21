class MissionBusinessException extends Exception {
    public MissionBusinessException(String message) {
        super(message);
    }
}

class MissionValidationException extends RuntimeException {
    public MissionValidationException(String message) {
        super(message);
    }
}