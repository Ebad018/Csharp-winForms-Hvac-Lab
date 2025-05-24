# 🧪 C# WinForms HVAC Testing Lab Software

This project is a **Windows Forms Application** developed in C# to automate and analyze data from HVAC Chiller Testing Labs. It communicates with Modbus RTU-compatible hardware via serial ports to collect temperature, pressure, humidity, and electrical readings in real-time and exports detailed reports as PDFs.

---

## 🔧 Features

- 📡 **Modbus RTU Serial Communication**  
  Supports multiple slave devices (ID 1 to 6), each collecting a specific type of sensor data.

- 📊 **Real-Time Sensor Monitoring**  
  - Temperature sensors (S1–S4, A1–A8, SH, SC, BS, CIWB)
  - Pressure and Humidity sensors
  - Electrical parameters (Voltage, Current, Power, EER)
  - Wet Bulb and Enthalpy-based calculations

- 📐 **BTU & Efficiency Calculations**  
  Computes:
  - Total BTU (tBTU)
  - Sensible BTU (sBTU)
  - Latent capacity
  - Superheat & Subcool using R410A PT chart

- 🧾 **PDF Report Generation**  
  - Saves reports with custom labels and input fields
  - Landscape layout grouped into Evaporator, Condenser, Electrical, and BTU data
  - Uses `iTextSharp`

- 🔁 **Auto Polling**  
  - Automatically cycles through slave devices at 1-second intervals
  - Can be toggled on/off from the UI

- ⚠️ **Error Handling & Alerts**  
  - Disconnected/short sensors marked visually
  - Red blinking light when tank reaches 1000ml
  - Live error display and log

---

## 🧰 Technologies Used

- **.NET Framework (Windows Forms)**
- **C#**
- **SerialPort Communication**
- **Modbus RTU**
- **iTextSharp** (PDF generation)
- **Multithreading**

---

## 🖥️ This is a Custom Made software that works with certian hardware and will only work on said hardware if correctly configured.
