# Cyber Security Task Assistant (Part 3)

## Overview

Cyber Security Task Assistant is a Java desktop application designed to help users improve cybersecurity awareness while managing personal tasks. The application combines task management, an interactive cybersecurity quiz, natural language processing (NLP), and activity logging into one intelligent assistant.

This project was developed as Part 3 of the Programming assignment.

---

## Features

### Task Management
- Add tasks using natural language.
- Store tasks automatically in `tasks.json`.
- Load saved tasks when the application starts.
- Mark tasks as completed.
- Delete tasks.
- Reminder support for every task.

### Cyber Security Quiz
- Launch quiz from a button or chat.
- More than 10 cybersecurity questions.
- Covers:
  - Phishing
  - Password Safety
  - Safe Browsing
  - Social Engineering
- One question displayed at a time.
- Immediate feedback after every answer.
- Score tracking.
- Final score and summary at the end.

### Activity Log
Records important user actions including:
- Task creation
- Task completion
- Task deletion
- Reminder creation
- Quiz started
- Quiz completed
- NLP actions

The assistant supports:
- Show activity log
- What have you done for me?
- Show more

### Natural Language Processing (NLP)

The assistant understands different ways of saying the same thing.

Examples:

- Add a task to study tomorrow
- Remind me to submit my assignment
- Start the quiz
- Begin the cybersecurity quiz
- Show activity log
- What have you done for me?

## Data Storage

The application stores information in JSON files.

Example:

- tasks.json
- activityLog.json

Data is automatically saved after every change.

---

## Technologies Used

- Java
- Swing GUI
- Gson
- JSON
- Object-Oriented Programming
- IntelliJ IDEA

---

## Project Structure

```
src/
│
├── GUI
├── NLP
├── Quiz
├── TaskManager
├── ActivityLogger
├── JSON
└── Main.java
```

---
