using ModelingSystem3.Bank.RulesImplementations.Model.Strategies;
using ModelingSystem3.Clinic.RulesImplementations.Elements;
using ModelingSystem3.Clinic.RulesImplementations.Elements.Internals;
using ModelingSystem3.Clinic.RulesImplementations.ModelObjects;
using ModelingSystem3.Clinic.RulesImplementations.Models.Strategies;
using ModelingSystem3.Clinic.RulesImplementations.Nodes;
using ModelingSystem3.Core.Elements;
using ModelingSystem3.Core.Models;
using ModelingSystem3.Core.Nodes;

namespace ModelingSystem3.Clinic
{
    public class ClinicSimulation
    {
        public void StartSimulation() {
            int simulationTime = 10000;

            ClinicClientFabric fabric = new ClinicClientFabric(new List<(ClinicClientType, double)>{
                (ClinicClientType.type_1, 0.5),
                (ClinicClientType.type_2, 0.1),
                (ClinicClientType.type_3, 0.4),
            });

            Create create = new Create("Create", 15, fabric);

            Process registration = new Process("Registration", 15);
            Process regToRoom = new Process("[Walking to a room] from registration to hospital room", 5.5);
            Process regToLabReg = new Process("[Walking to a room] from registration to laboratory registration", 3.5);
            Process labToReg = new Process("[Walking to a room] from laboratory to registration", 3.5);
            Process laboratoryRegistration = new Process("Laboratory registration", 4.5);
            Process laboratory = new Process("Laboratory", 4);

            ClinicDispose hospitalRoom = new ClinicDispose("Hospital room");
            ClinicDispose exitFromHospital = new ClinicDispose("Exit from the hospital");

            CommonNode NodeReg = new CommonNode(new List<AbstractElement> { registration });

            ClinicBranchingNode NodeLabOrRoomBranch = new ClinicBranchingNode(new List<(ClinicClientType, AbstractElement)> {
                (ClinicClientType.type_1,regToRoom),
                (ClinicClientType.type_2,regToLabReg),
                (ClinicClientType.type_3,regToLabReg),
            }, regToRoom);

            CommonNode NodeLabReg = new CommonNode(new List<AbstractElement> { laboratoryRegistration });

            CommonNode NodeLab = new CommonNode(new List<AbstractElement> { laboratory });

            CommonNode NodeHospitalRoom = new CommonNode(new List<AbstractElement> { hospitalRoom });

            ClinicBranchingNode NodeRegOrExitBranch = new ClinicBranchingNode(new List<(ClinicClientType, AbstractElement)> {
                (ClinicClientType.type_1,labToReg),
                (ClinicClientType.type_2,labToReg),
                (ClinicClientType.type_3,exitFromHospital),
            }, exitFromHospital);

            List<AbstractElement> elements = new List<AbstractElement> {
                create,
                registration,
                regToRoom,
                regToLabReg,
                labToReg,
                laboratoryRegistration,
                laboratory,
                hospitalRoom,
                exitFromHospital
             };

            create.SetDistribution(DistributionType.EXP);
            create.SetNode(NodeReg);

            RegistrationTimeEstimator timeEstimator = new RegistrationTimeEstimator(15);
            timeEstimator.SetProcessingTime(ClinicClientType.type_1, 15);
            timeEstimator.SetProcessingTime(ClinicClientType.type_2, 40);
            timeEstimator.SetProcessingTime(ClinicClientType.type_3, 30);

            registration.SetDistribution(DistributionType.EXP);
            registration.SetTimeEstimator(timeEstimator);
            registration.SetWorkers(2);
            registration.SetNode(NodeLabOrRoomBranch);

            regToRoom.SetDistribution(DistributionType.UNIF);
            regToRoom.SetWorkers(3);
            regToRoom.SetDelayDev(2.5);
            regToRoom.SetNode(NodeHospitalRoom);

            regToLabReg.SetDistribution(DistributionType.UNIF);
            regToLabReg.SetWorkers(200);
            regToLabReg.SetDelayDev(1.5);
            regToLabReg.SetNode(NodeLabReg);


            ClinicClientMutator mutator = new ClinicClientMutator(new List<(ClinicClientType, ClinicClientType)> {
                (ClinicClientType.type_1,ClinicClientType.type_1),
                (ClinicClientType.type_2,ClinicClientType.type_1),
                (ClinicClientType.type_3,ClinicClientType.type_3),
            });
            labToReg.SetDistribution(DistributionType.UNIF);
            labToReg.SetWorkers(200);
            labToReg.SetDelayDev(1.5);
            labToReg.SetOnActObjectMutator(mutator);
            labToReg.SetNode(NodeReg);

            laboratoryRegistration.SetDistribution(DistributionType.ERLANG);
            laboratoryRegistration.SetShape(3);
            laboratoryRegistration.SetNode(NodeLab);

            laboratory.SetDistribution(DistributionType.ERLANG);
            laboratory.SetWorkers(2);
            laboratory.SetShape(2);
            laboratory.SetNode(NodeRegOrExitBranch);

            List<INode> nodes = new List<INode> {
                NodeReg,
                NodeLabOrRoomBranch,
                NodeLabReg,
                NodeLab,
                NodeHospitalRoom,
                NodeRegOrExitBranch
             };

            ClinicStatisticsStrategy statisticsStrategy = new ClinicStatisticsStrategy(simulationTime);

            Model model = new Model(elements, nodes, statisticsStrategy);

            model.Simulate(simulationTime, true);
        }
    }
}
