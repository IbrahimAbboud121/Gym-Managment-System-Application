# 🏋️ GYM MANAGEMENT SYSTEM  
*Built with C# & OOP*

A complete gym administration solution that handles memberships, trainer scheduling, class bookings, and capacity management.

---

## ✨ FEATURES
- **Member Management** – Add, view, update, and remove members
- **Trainer Management** – Add and remove trainers with specializations
- **Class Booking** – Real-time slot tracking with capacity limits
- **Smart Validation** – Block bookings for expired/inactive members
- **Attendance Reports** – Generate class participation lists

---

## 🧱 CORE CLASSES
```csharp
Member.cs        // ID, name, membership type, status
Trainer.cs       // ID, name, specialization (Yoga, HIIT, etc.)
ClassSession.cs  // Class name, trainer, schedule, max slots, booked members
GymManager.cs    // Main system operations

⚙️ HOW IT WORKS
Admin creates a class (e.g., "Zumba" with 20 slots)
Trainer is assigned to the class
Active member books → slots decrease
Expired member tries → booking rejected
Admin can view attendance reports

