import React from 'react';
import { Navbar, Nav, Container } from 'react-bootstrap';
import { Link, useNavigate } from 'react-router-dom';

const NavigationBar = () => {
  const navigate = useNavigate(); // Use for redirecting
  const storedUser = localStorage.getItem('user');
  let user = null;

  // Safely parse the JSON string from localStorage
  try {
    user = storedUser ? JSON.parse(storedUser) : null;
  } catch (error) {
    console.error('Failed to parse user from localStorage', error);
  }

  const isAuthenticated = user !== null;

  // Logout handler
  const handleLogout = () => {
    localStorage.removeItem('user');  // Clear user data from localStorage
    window.location.reload(); // Reload the page to reflect changes in the navbar
    navigate('/login'); // Redirect to login page if needed
  };

  return (
    <Navbar expand="lg" sticky="top" style={{ backgroundColor: '#00B98E' }}>
      <Container>
        <Navbar.Brand as={Link} to="/">
          <img
            src="/images/logo.png"
            alt="My App Logo"
            width="131"
            height="100%"
            className="d-inline-block align-top"
          />
        </Navbar.Brand>
        
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            <Nav.Link as={Link} to="/" style={{ color: 'white' }}>Home</Nav.Link>
            <Nav.Link as={Link} to="/activities" style={{ color: 'white' }}>Activities</Nav.Link>
            <Nav.Link as={Link} to="/abonnements" style={{ color: 'white' }}>Abonnements</Nav.Link>

            {/* Conditionally show Profile and Reservation links if authenticated */}
            {isAuthenticated && (
              <>
                <Nav.Link as={Link} to="/profile" style={{ color: 'white' }}>Profile</Nav.Link>
                <Nav.Link as={Link} to="/reservation" style={{ color: 'white' }}>Reservation</Nav.Link>
                <Nav.Link onClick={handleLogout} style={{ color: 'white', cursor: 'pointer' }}>Logout</Nav.Link>
              </>
            )}

            {!isAuthenticated && (
              <Nav.Link as={Link} to="/login" style={{ color: 'white' }}>Login</Nav.Link>
            )}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default NavigationBar;
