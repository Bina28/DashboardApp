import { useState, useEffect } from "react";
import api from "../api/api";
import { useNavigate } from "react-router-dom";

export default function PaymentsHistory() {
  const [payments, setPayments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    async function fetchPayments() {
      try {
        const response = await api.get("/payments?take=5");
        setPayments(response.data);
      } catch (err) {
        setError("Ошибка при загрузке платежей");
      } finally {
        setLoading(false);
      }
    }
    fetchPayments();
  }, []);

  if (loading) return <p>Загрузка платежей...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div className="max-w-4xl mx-auto p-6 bg-white rounded-lg shadow-md">
      <button
        onClick={() => navigate("/dashboard")}
        className="mb-6 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
      >
        Назад на Dashboard
      </button>

      <h2 className="text-2xl font-bold mb-6 text-center text-gray-800">
        История платежей
      </h2>
      <div className="overflow-x-auto">
        <table className="min-w-full border border-gray-200 text-sm text-left">
          <thead className="bg-gray-100 text-gray-700">
            <tr>
              <th className="p-3 border-b border-gray-300">Дата</th>
              <th className="p-3 border-b border-gray-300">Сумма</th>
              <th className="p-3 border-b border-gray-300">Клиент</th>
              <th className="p-3 border-b border-gray-300">Email клиента</th>
            </tr>
          </thead>
          <tbody>
            {payments.map((p) => (
              <tr
                key={p.id}
                className="border-b border-gray-200 hover:bg-gray-50"
              >
                <td className="p-3">{new Date(p.date).toLocaleDateString()}</td>
                <td className="p-3">{p.amount} NOK</td>
                <td className="p-3">{p.clientName}</td>
                <td className="p-3">{p.clientEmail}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
