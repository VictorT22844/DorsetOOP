﻿using DorsetOOP.Models;
using DorsetOOP.Models.Users;
using DorsetOOP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DorsetOOP
{
    /// <summary>
    /// Interaction logic for TeacherView.xaml
    /// </summary>
    public partial class TeacherView : Window, INotifyPropertyChanged
    {
        #region View Models
        public event PropertyChangedEventHandler PropertyChanged;

        private Teacher _loggedInTeacher;
        public Teacher LoggedInTeacher
        {
            get { return _loggedInTeacher; }
            set
            {
                _loggedInTeacher = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LoggedInTeacher"));
            }
        }

        private ObservableCollection<Student> _students = new ObservableCollection<Student>();
        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Students"));
            }
        }

        private string _searchStudentTextBox;
        public string SearchStudentTextBox
        {
            get { return _searchStudentTextBox; }
            set
            {
                _searchStudentTextBox = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SearchStudentTextBox"));
                if (SearchStudentTextBox != "" && SearchStudentTextBox != "") Students = new ObservableCollection<Student>(VirtualCollegeContext.GetAllStudentsOfTutorThatMatchFullName(LoggedInTeacher, SearchStudentTextBox));
            }
        }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get { return _selectedStudent; }

            set
            {
                _selectedStudent = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedStudent"));
            }
        }

        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get { return _selectedCourse; }
            set
            {
                _selectedCourse = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCourse"));
                if (LoggedInTeacher != null && SelectedCourse != null)
                {
                    TeacherLessons = new ObservableCollection<Lesson>(VirtualCollegeContext.GetLessonsFromCourse(LoggedInTeacher, SelectedCourse));
                    Grades = new ObservableCollection<Grade>(VirtualCollegeContext.GetAllGrades(SelectedCourse));
                }
            }
        }


        private ObservableCollection<Lesson> _teacherLessons;
        public ObservableCollection<Lesson> TeacherLessons
        {
            get { return _teacherLessons; }
            set
            {
                _teacherLessons = value;
                PropertyChanged(this, new PropertyChangedEventArgs("TeacherLessons"));
            }
        }
        private Lesson _selectedLesson;
        public Lesson SelectedLesson
        {
            get { return _selectedLesson; }
            set
            {
                _selectedLesson = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedLesson"));
            }
        }
        private Grade _selectedGrade = new Grade();
        public Grade SelectedGrade
        {
            get { return _selectedGrade; }
            set
            {
                _selectedGrade = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedGrade"));
            }
        }

        private ObservableCollection<Grade> _grades;
        public ObservableCollection<Grade> Grades
        {
            get { return _grades; }
            set
            {
                _grades = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Grades"));
            }
        }

        #endregion

        public TeacherView(User _inputUser)
        {
            InitializeComponent();
            LoggedInTeacher = (Teacher)_inputUser;
            Students = new ObservableCollection<Student>(LoggedInTeacher.Tutoring);

        }
        private void Students_Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            new StudentDetailsView(SelectedStudent).ShowDialog();
        }

        private void deleteGradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGrade != null)
            {
                VirtualCollegeContext.RemoveGrade(SelectedGrade);
                MessageBox.Show("Grade deleted!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                SelectedCourse = SelectedCourse;
                Grades = new ObservableCollection<Grade>(VirtualCollegeContext.GetAllGrades(SelectedCourse));
            }
            else MessageBox.Show("Please select a grade", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void addGradeButton_Click(object sender, RoutedEventArgs e)
        {
            new AddGradeView(SelectedCourse).ShowDialog();
            Grades = new ObservableCollection<Grade>(VirtualCollegeContext.GetAllGrades(SelectedCourse));
        }

        private void allGradesOfCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void allGradesOfCourse_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new EditGradeView(SelectedGrade).ShowDialog();
            VirtualCollegeContext.UpdateGrade(SelectedGrade);
        }
    }
}
