import React from 'react';
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import LoginSignUp from './Components/LoginSignUp/LoginSignUp';
import Main from './Components/Main/Main';
import ForgotPassword from './Components/ForgotPassword/ForgotPassword';
import CreateEvent from './Components/CreateEvent/CreateEvent';
import CreateCalendar from './Components/CreateCalendar/CreateCalendar';
import CalendarView from './Components/CalendarView/CalendarView';
import SharePage from './Components/SharePage/SharePage';

function App() {
  return (
    <Router>
      <div>
        <Routes>
          <Route path="/" element={<LoginSignUp />} />
          <Route path="/home" element={<Main />} />
          <Route path="/calendar" element={<CalendarView />} />
          <Route path="/reset-password" element={<ForgotPassword />} />
          <Route path="/create-event" element={<CreateEvent />} />
          <Route path="/create-calendar" element={<CreateCalendar />} />
          <Route path="/share" element={<SharePage />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
