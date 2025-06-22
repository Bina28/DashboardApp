import LoginPage from "./components/LoginPage";
import DashboardPage from "./components/DashboardPage";
import "./App.css";
import { BrowserRouter, Route, Routes, Link } from "react-router-dom";
import PaymentsHistory from "./components/PaymentHistoryPage";
import ExchangeRateSection from "./components/ExchangeRateSection";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        s
        <Route path="/" element={<LoginPage />} />
        <Route path="/dashboard" element={<DashboardPage />} />
        <Route path="/rates" element={<ExchangeRateSection />} />
        <Route path="/payments-history" element={<PaymentsHistory />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
