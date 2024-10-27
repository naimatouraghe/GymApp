import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import { Container, Row, Col, Card, Button, Form, Alert } from 'react-bootstrap';

const Paiement = () => {
  const { abonnementId } = useParams();
  const [abonnement, setAbonnement] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [paymentSuccess, setPaymentSuccess] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    console.log('Fetching abonnement with ID:', abonnementId);
    axios.get(`https://localhost:7126/api/Abonnement/${abonnementId}`)
      .then(response => {
        setAbonnement(response.data);
        console.log('Fetched abonnement:', response.data);
      })
      .catch(error => {
        console.error('Error fetching abonnement:', error.response ? error.response.data : error.message);
        setError('Could not fetch abonnement details.');
      })
      .finally(() => setLoading(false));
  }, [abonnementId]);

  const handlePayment = async (event) => {
    event.preventDefault();

    // Retrieve user ID from local storage
    const userData = localStorage.getItem('user');
    if (!userData) {
      setError('User not found in local storage.');
      return;
    }

    const utilisateurId = JSON.parse(userData).id; // Assuming user object has an 'id' field
    const abonnementIdNumber = parseInt(abonnementId, 10);

    // Prepare payment data
    const paymentData = {
      Prix: abonnement.prix,
      UtilisateurId: utilisateurId,
      AbonnementId: abonnementIdNumber,
      TypeAbonnement: abonnement.type // Assuming 'type' represents the type of abonnement
    };
    console.log('Abonnement Type:', abonnement.type);
    // Validate payment data
    if (!paymentData.Prix || !paymentData.UtilisateurId || !paymentData.AbonnementId || !paymentData.TypeAbonnement) {
      setError('Please fill in all fields correctly.');
      return;
    }

    try {
      // Send payment request to the API
      const response = await axios.post(`https://localhost:7126/api/Paiement`, paymentData);
      const createdPaymentId = response.data.id; // Assuming your API returns the created payment ID
      console.log('Created payment ID:', createdPaymentId);

      setPaymentSuccess(true);
      setTimeout(() => {
        navigate('/abonnements');
      }, 2000);
    } catch (error) {
      console.error('Payment processing error:', error);
      setError('Error processing payment. Please try again.');
    }
  };

  if (loading) {
    return <p>Loading abonnement details...</p>;
  }

  if (error) {
    return <Alert variant="danger">{error}</Alert>;
  }

  return (
    <Container className="payment-container">
      <Row className="my-4">
        <Col>
          <h2 style={{ color: '#00B98E', fontWeight: 'bold', textAlign: 'center' }}>
            Payment for {abonnement.type.charAt(0).toUpperCase() + abonnement.type.slice(1)} Plan
          </h2>
        </Col>
      </Row>

      {paymentSuccess ? (
        <Alert variant="success">
          Payment successful! Thank you for your subscription. Redirecting...
        </Alert>
      ) : (
        <Card className="payment-card">
          <Card.Body>
            <Card.Title className="my-4">Enter Payment Details</Card.Title>
            <Form onSubmit={handlePayment}>
              <Form.Group controlId="formCardNumber">
                <Form.Label>Card Number</Form.Label>
                <Form.Control type="text" placeholder="Enter your card number" required />
              </Form.Group>

              <Form.Group controlId="formCardExpiry" className="my-3">
                <Form.Label>Expiry Date</Form.Label>
                <Form.Control type="text" placeholder="MM/YY" required />
              </Form.Group>

              <Form.Group controlId="formCardCVC" className="my-3">
                <Form.Label>CVC</Form.Label>
                <Form.Control type="text" placeholder="Enter CVC" required />
              </Form.Group>

              <Button variant="primary" type="submit">
                Confirm Payment
              </Button>
            </Form>
          </Card.Body>
        </Card>
      )}
    </Container>
  );
};

export default Paiement;
