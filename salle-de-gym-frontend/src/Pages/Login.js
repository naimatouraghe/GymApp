import React, { useState } from 'react';
import axios from 'axios';
import { Form, Button, Alert, Container, Row, Col } from 'react-bootstrap';

function LoginForm() {
  const [email, setEmail] = useState('');
  const [motDePasse, setMotDePasse] = useState(''); // Using motDePasse for password

  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();

    // Clear previous messages
    setError('');
    setSuccess('');

    // Basic validation (only email and password required)
    if (!email || !motDePasse) {
      setError('Please fill out all fields.');
      return;
    }

    const credentials = {
      email,
      motDePasse  // Ensure this matches the backend expectation
    };

    // Send login request to the API
    axios.post('https://localhost:7126/api/Utilisateur/login', credentials)
      .then(response => {
        const { user, token } = response.data; // Assuming backend returns user and token separately
        setSuccess('Login successful!');
        setError('');

        // Save user data and token separately in localStorage
        localStorage.setItem('user', JSON.stringify(user));
        localStorage.setItem('token', token);

        // Redirect to the profile or home page
        window.location.href = '/profile'; // Example redirect after login
      })
      .catch(error => {
        setError('Invalid credentials. Please try again.');
        console.error('Login error', error);
      });
  };

  return (
    <Container>
      <Row className="justify-content-md-center my-4">
        <Col md={6}>
          <h2 className="text-center">Login</h2>

          {error && <Alert variant="danger">{error}</Alert>}
          {success && <Alert variant="success">{success}</Alert>}

          <Form onSubmit={handleSubmit}>
            <Form.Group controlId="formEmail" className="mb-3">
              <Form.Label>Email</Form.Label>
              <Form.Control
                type="email"
                placeholder="Enter your email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </Form.Group>

            <Form.Group controlId="formPassword" className="mb-3">
              <Form.Label>Password</Form.Label>
              <Form.Control
                type="password"
                placeholder="Enter your password"
                value={motDePasse}  // Use motDePasse for the input value
                onChange={(e) => setMotDePasse(e.target.value)}
                required
              />
            </Form.Group>

            <Button variant="primary" type="submit" className="w-100">
              Login
            </Button>
          </Form>
        </Col>
      </Row>
    </Container>
  );
}

export default LoginForm;
