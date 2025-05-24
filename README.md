🧩 Core Functionality
The application:

Communicates with multiple Modbus RTU slaves via SerialPort.

Continuously reads temperature, pressure, humidity, voltage, current, and other sensor data.

Parses incoming bytes, validates them via CRC, and updates the UI with real-time data.

Calculates:

Wet bulb temperature

Enthalpy (via lookup table)

Total and sensible BTU

Latent capacity

Superheat and Subcool values using a PT chart (R410A)

Exports all test results as a well-formatted PDF report using iTextSharp.

🔌 Hardware Integration
Reads data from 5 different slave devices:

Slave 1: Temperature sensors (S1–S4, SH, SC, etc.)

Slave 2: Additional temperature sensors (A1–A8)

Slave 3: Pressure sensors (P1, P2), ultrasonic sensors, RH1, etc.

Slave 5: Humidity, discharge/suction temp sensors

Slave 6: Electrical parameters (Voltage, Current, Power, EER)

📈 UI Features
Real-time data display in textboxes.

Dynamic COM port selection and reconnect logic.

Auto-querying through all slave devices using a timer.

Visual alerts for disconnected sensors or port errors.

Blink indicators for critical states (e.g., tank full).

Live update of calculated values like BTU, superheat, and subcooling.

📄 PDF Export
Generates a lab test report PDF (in landscape mode).

Saves to a folder on desktop.

Includes: Engineer name, unit specs, sensor readings, electrical data, and calculated BTU values.

Draws group boxes for logical data grouping (Evaporator, Condenser, Electrical, BTU).

🛠️ Other Utilities
Temperature conversion between °C and °F

PT Chart and Enthalpy lookup

Serial port refresh

UI scaling for dynamic window resizing

