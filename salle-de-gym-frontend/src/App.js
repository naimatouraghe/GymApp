import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';

import Signup from './Pages/Signup';

import LoginForm from './Pages/Login';
import NavigationBar from './components/Navbar/Navbar';  // Import Navbar component
import 'bootstrap/dist/css/bootstrap.min.css';
import Home from './Pages/Home/Home';
import Activities from './Pages/Activities/Activities';
import Abonnement from './Pages/Abonnement/Abonnement';
import Reservation from './Pages/Reservations';
import Profile from './Pages/Profile';

import ActivityDetail from './Pages/ActivityDetail/ActivityDetail';
import Paiement from './Pages/Abonnement/Paiement';



function App() {
  
  return (
    <Router>
      <div className="App">
        {/* Include Navbar component */}
        <NavigationBar />

        <main>
          <Routes>
            <Route path="/" element={<Home/>} />
            <Route path="/signup" element={<Signup />} />
            <Route path="/login" element={<LoginForm />} />
            <Route path="/activities" element={<Activities />} />
            <Route path="/abonnements" element={<Abonnement />} />
            <Route path="/paiement/:abonnementId" element={<Paiement />} />
            <Route path="/profile" element={<Profile/>} />
            <Route path="/reservation" element={<Reservation />} />
            <Route path="/activities/:id" element={<ActivityDetail/>} />
           
          </Routes>
        </main>

      
      </div>
    </Router>
  );
}

export default App;
